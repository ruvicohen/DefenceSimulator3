using DefenceSimulator3.Data;
using DefenceSimulator3.Models;
using DefenceSimulator3.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DefenceSimulator3.Sockets
{
    public class IronDomeHub: Hub
    {
        private readonly AttackHandlerService _attackHandlerService;
        private readonly DefenceSimulator3Context _context;

        public IronDomeHub(AttackHandlerService attackHandlerService, DefenceSimulator3Context context)
        {
            _attackHandlerService = attackHandlerService;
            _context = context;
        }
        public async Task sendmissle()
        {
            string result = await _attackHandlerService.Interception();
            await Clients.All.SendAsync("SpecialMessage", $"{result}");
            int defenceAmount = (await _context.WeaponDefence.FirstOrDefaultAsync()).Amount;
            await Clients.All.SendAsync("ReceiveAmount", defenceAmount);

        }
        public async Task FailAttack(int threatId)
        {
            Threat threat = await _context.Threat.FindAsync(threatId);
            await _attackHandlerService.RemoveAttackForFail(threat);
        }
        public async Task TimeToImpact(int threatId, string timeToImpact)
        {
            Threat threat = await _context.Threat.FindAsync(threatId);
            await _attackHandlerService.UpdateTimeToImpact(threat.ThreatId, int.Parse(timeToImpact));
        }
        public async Task SendInitialRequest()
        {
            await _attackHandlerService.loadQueue();
            await _attackHandlerService.SendThreats();
        }
    }
}
