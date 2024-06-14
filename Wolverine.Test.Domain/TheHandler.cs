namespace Wolverine.Test.Domain;

public class TheHandler
{
    public async Task Handle(MyCommand cmd, IMessageBus bus)
    {
        Console.WriteLine($"Start HandleMyCommand {cmd.Id}");
        await bus.PublishAsync(new MyCommandToDelay { Id = cmd.Id },
            new DeliveryOptions { ScheduleDelay = TimeSpan.FromSeconds(10) });
    }

    public void Handle(MyCommandToDelay cmd)
    {
        Console.WriteLine($"Start HandleMyCommandToDelay {cmd.Id}");
    }
}