using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorBugTimePicker.Pages
{
    public partial class MyTimePicker
    {
        [Parameter]
        public TimeSpan? Min { get; set; }
        [Parameter]
        public TimeSpan? Max { get; set; }
        [Parameter]
        public DateTime Value { get; set; }
        [Parameter]
        public EventCallback<DateTime> ValueChanged { get; set; }
        [Parameter]
        public bool Disabled { get; set; }

        [Parameter]
        public int? Step { get; set; }

        protected Guid Id { get; private set; } = Guid.NewGuid();

        [Parameter]
        public string Label { get; set; }

        private DateTime _internalValue;
        public DateTime InternalValue
        {
            get
            {
                return _internalValue;
            }
            set
            {
                if(_internalValue.Hour != value.Hour || _internalValue.Minute != value.Minute)
                {
                    _internalValue = _internalValue.Date.AddHours(value.Hour).AddMinutes(value.Minute);
                    this.Value = _internalValue;
                    this.ValueChanged.InvokeAsync(this.Value);
                }
            }
        }

        protected override void OnParametersSet()
        {
            _internalValue = this.Value;

            if(this.Min.HasValue && this.Value.ToDayTimeSpan() < this.Min)
            {
                this.InternalValue = this.Value.ToTime(this.Min.Value);
            }
            else if (this.Max.HasValue && this.Value.ToDayTimeSpan() > this.Max)
            {
                this.InternalValue = this.Value.ToTime(this.Max.Value);
            }


            base.OnParametersSet();
        }
    }

    public static class DateTimeExtensions
    {
     
        public static TimeSpan ToDayTimeSpan(this DateTime source)
        {
            return new TimeSpan(source.Hour, source.Minute, source.Second);
        }

        public static DateTime ToTime(this DateTime source, TimeSpan hours)
        {
            return source.ToTime(hours.Hours, hours.Minutes, hours.Seconds);
        }

        public static DateTime ToTime(this DateTime source, int hours, int minutes, int seconds)
        {
            return source.Date.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
        }
    }
}
