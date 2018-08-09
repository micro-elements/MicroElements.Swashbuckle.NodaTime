using System;
using Microsoft.AspNetCore.Mvc;
using NodaTime;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NodaTimeValuesController : ControllerBase
    {
        [HttpGet("[action]")]
        public ActionResult<NodaTimeModel> GetModel()
        {
            var dateTimeZone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            Instant instant = Instant.FromDateTimeUtc(DateTime.UtcNow);
            ZonedDateTime zonedDateTime = instant.InZone(dateTimeZone);
            NodaTimeModel nodaTimeModel = new NodaTimeModel
            {
                DateTimeZone = DateTimeZone.Utc,
                Instant = instant,
                DateTime = DateTime.UtcNow,
                Interval = new Interval(instant, instant.Plus(Duration.FromHours(1))),
                DateInterval = new DateInterval(zonedDateTime.Date, zonedDateTime.Date.PlusDays(1)),
                Period = Period.FromHours(1),
                ZonedDateTime = zonedDateTime,
                OffsetDateTime = instant.WithOffset(Offset.FromHours(1)),
                LocalDate = zonedDateTime.Date,
                LocalTime = zonedDateTime.TimeOfDay,
                LocalDateTime = zonedDateTime.LocalDateTime,
                Offset = zonedDateTime.Offset,
                Duration = Duration.FromHours(1)
            };
            return nodaTimeModel;
        }
    }
}
