using System.Collections.Concurrent;
namespace Chapter_7___Dependency_Injection_In_Minimal_APIs
{
    public class DeliveryDateBookingService
    {
        private ConcurrentQueue<DateTime> _availableDates = new ConcurrentQueue<DateTime>();
        public DeliveryDateBookingService()
        {
            _availableDates.Enqueue(DateTime.Now.AddDays(1));
            _availableDates.Enqueue(DateTime.Now.AddDays(2));
            _availableDates.Enqueue(DateTime.Now.AddDays(3));
            _availableDates.Enqueue(DateTime.Now.AddDays(4));
            _availableDates.Enqueue(DateTime.Now.AddDays(5));
        }
        public DateTime GetNextAvailableDate()
        {
            if (_availableDates.Count == 0)
            {
                throw new Exception("No Dates Available");
            }
            var dequeuedDate = _availableDates.TryDequeue(out var result);
            if (dequeuedDate == false)
            {
                throw new Exception("An error occured");
            }
            return result;
        }
    }
}
