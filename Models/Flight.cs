﻿namespace AirportServer.Models
{
    public class Flight
    {

        public Flight() => LegTimer = new Timer(MyCallback);

        public int Id { get; set; }
        public string Code { get; set; } = "SomeFlight";
        public StatusType Status { get; set; }
        private Leg? leg { get; set; }
        public virtual Leg? Leg
        {
            get => leg; set
            {
                leg = value;

                if (leg != null)
                {
                    LegTimer.Change(0, leg.CrossingTime * 3);
                }
            }
        }

        public Timer LegTimer;

        private void MyCallback(object? state)
        {
            CallbackCalled?.Invoke(this);
        }

        public event Action<Flight> CallbackCalled;


    }
}