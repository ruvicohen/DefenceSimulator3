using DefenceSimulator3.Data;
using DefenceSimulator3.Models;
using DefenceSimulator3.Sockets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuGet.Common;
using System.Collections.Concurrent;
using System.Reflection.Emit;
using System.Threading;
using static DefenceSimulator3.Models.Enums;

namespace DefenceSimulator3.Service
{
    public class AttackHandlerService
    {
        private static ConcurrentDictionary<int, CancellationTokenSource> _attacks = new ConcurrentDictionary<int, CancellationTokenSource>();
        public static ConcurrentQueue<Threat> _threatQueue = new ConcurrentQueue<Threat>();
        public static ConcurrentQueue<Threat> _threatDoneQueue = new ConcurrentQueue<Threat>();
        public static ConcurrentQueue<Threat> _threatInterceptedQueue = new ConcurrentQueue<Threat>();

        private readonly DefenceSimulator3Context _context;
        private readonly IHubContext<IronDomeHub> _hubContext;


        public AttackHandlerService(DefenceSimulator3Context ctx, IHubContext<IronDomeHub> hubContext)
        {
            _context = ctx;
            _hubContext = hubContext;

        }


        public async Task<bool> RegisterAndRunAttackTask(int threatId)
        {
            var threat = await _context.Threat.Include(t => t.Origin).Include(t => t.Weapon).FirstOrDefaultAsync(t => t.ThreatId == threatId);
            if (threat.TimeToImpact1 == -1)
            {
                threat.LaunchTime = DateTime.Now;
            }
            

            threat.Status = Enums.ThreatStatus.פעיל;
            await _context.SaveChangesAsync();
            var cts = new CancellationTokenSource();
            _attacks[threat.ThreatId] = cts;
            await loadQueue();
            await SendThreats();

            /*green non awaited warning without _ = */
            _ = Task.Run(() => RunTask(threat, cts.Token), cts.Token);


            return true;
        }

        public async Task<bool> RemoveAttackForIntercepted(int threatId)
        {
            var threat = await _context.Threat.FirstOrDefaultAsync(t => t.ThreatId == threatId);
            _attacks.TryRemove(threat.ThreatId, out CancellationTokenSource? cts);
            cts?.Cancel();

            threat.Status = Enums.ThreatStatus.הסתיים;
            await _context.SaveChangesAsync();
            Console.WriteLine("remove attack");

            return true;
        }
        
        public async Task<bool> RemoveAttackForFail(Threat threat)
        {

            _attacks.TryRemove(threat.ThreatId, out CancellationTokenSource? cts);
            cts?.Cancel();
            threat.Status = Enums.ThreatStatus.הסתיים;
            threat.Fail = threat.Amount - threat.Success;
            await _context.SaveChangesAsync();
            _threatQueue.TryDequeue(out _);
            await loadQueue();
            await SendThreats();


            return true;
        }
        public async Task<string> Interception()
        {
            if (_threatQueue.TryPeek(out Threat frontThreat))
            {
                string validationResult = await ValidateAndPrepareInterception(frontThreat.ThreatId);
                if (validationResult != "Success") return validationResult;
                var updatedThreat = await _context.Threat.FindAsync(frontThreat.ThreatId);
                if (updatedThreat != null)
                {
                    // Update the threat object in the queue with the latest values from the database
                    frontThreat.Amount = updatedThreat.Amount;
                    frontThreat.Success = updatedThreat.Success;

                    if ((frontThreat.Amount - frontThreat.Success) == 0)
                    {
                        await RemoveAttackForIntercepted(frontThreat.ThreatId);
                        _threatQueue.TryDequeue(out _);
                    }
                }

                await loadQueue();
                await SendThreats();
            }
            return "יורט בהצלחה";
        }
        private async Task RunTask(Threat threat, CancellationToken token)
        {
            try
            {
                using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(1));
                int? elapsedSeconds;
                if (threat.TimeToImpact1 != -1) {

                    elapsedSeconds = threat.TimeToImpact1;
                }
                else
                {
                    elapsedSeconds = threat.TimeToImpact;
                }
                while (await timer.WaitForNextTickAsync(token) && elapsedSeconds > 0)
                {
                    elapsedSeconds -= 1;
                    var message = $"Attack {threat.ThreatId} will explode in {elapsedSeconds} seconds.";
                    Console.WriteLine(message);
                    await _hubContext.Clients.All.SendAsync("UpdateTimerExplode", threat.ThreatId, elapsedSeconds);
                }
            }
            catch (TaskCanceledException)
            {
            }
            finally
            {

            }
        }
        private async Task<string> ValidateAndPrepareInterception(int id)
        {
            Threat threat = await _context.Threat.FindAsync(id);
            WeaponDefence weaponDefence = await _context.WeaponDefence.FirstOrDefaultAsync();
            if (weaponDefence.Amount <= 0)
                return "לא מספיק תחמושת";

            if (threat == null)
            {
                weaponDefence.Amount -= 1;
                return "שגיאה";
            }
            if (threat.Amount <= 0)
            {
                weaponDefence.Amount -= 1;
                return "שגיאה";
            }

            weaponDefence.Amount -= 1;
            threat.Success += 1;

            await _context.SaveChangesAsync();
            return "Success";
        }
        public async Task InitialThreats()
        {
            await loadQueue();
            foreach(Threat threat in _threatQueue)
            {
                DateTime LaunchAttack = threat.LaunchTime;
                TimeSpan LaunchToNow = LaunchAttack - DateTime.Now;
                int? timeToImpact = threat.TimeToImpact;
                int SecendsFromLaunchToNow = (LaunchToNow.Days * 86400) + (LaunchToNow.Hours * 3600) + (LaunchToNow.Minutes * 60) + LaunchToNow.Seconds;
                if ((SecendsFromLaunchToNow + timeToImpact) > 0)
                {
                    threat.TimeToImpact1 = timeToImpact + SecendsFromLaunchToNow;
                    await RegisterAndRunAttackTask(threat.ThreatId);
                }
                else
                {
                    await RemoveAttackForFail(threat);
                }
            }

        }
        public async Task SendThreats()
        {
            await _hubContext.Clients.All.SendAsync("BE_ReciveThreatsQueue", _threatQueue);
            await _hubContext.Clients.All.SendAsync("ReceiveQueueFail", _threatDoneQueue);
            await _hubContext.Clients.All.SendAsync("ReceiveQueueIntercepted", _threatInterceptedQueue);

        }

        public async Task loadQueue()
        {
            _threatQueue = await LoadThreatsByStatus(ThreatStatus.פעיל);
            _threatDoneQueue = await LoadThreatsDoneByStatus(ThreatStatus.הסתיים);
            _threatInterceptedQueue = await LoadThreatsInterceptedByStatus(ThreatStatus.הסתיים);
        }


        private async Task<ConcurrentQueue<Threat>> LoadThreatsByStatus(ThreatStatus status)
        {
            List<Threat> threatsFromDb = await _context.Threat.Include(t => t.Weapon).Include(t => t.Origin).Where(t => t.Status == status).Where(t => t.Amount > 0).ToListAsync();
            return new ConcurrentQueue<Threat>(threatsFromDb);
        }
        private async Task<ConcurrentQueue<Threat>> LoadThreatsDoneByStatus(ThreatStatus status)
        {
            List<Threat> threatsFromDb = await _context.Threat.Include(t => t.Weapon).Include(t => t.Origin).Where(t => t.Status == status).Where(t => t.Fail > 0).ToListAsync();
            return new ConcurrentQueue<Threat>(threatsFromDb);
        }
        private async Task<ConcurrentQueue<Threat>> LoadThreatsInterceptedByStatus(ThreatStatus status)
        {
            List<Threat> threatsFromDb = await _context.Threat.Include(t => t.Weapon).Include(t => t.Origin).Where(t => t.Success > 0).ToListAsync();
            return new ConcurrentQueue<Threat>(threatsFromDb);
        }
        public async Task<bool> UpdateTimeToImpact(int threatId, int timeToImpact)
        {
            var threat = await _context.Threat.FirstOrDefaultAsync(t => t.ThreatId == threatId);

            threat.TimeToImpact1 = timeToImpact;
            await _context.SaveChangesAsync();

            return true;
        }



    }
}
