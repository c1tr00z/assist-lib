using System;
using System.Collections.Generic;
using System.Linq;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {

    public class DateSelectExample : DataModelBase {
        
        [ReferenceType(typeof(object))]
        [SerializeField]
        private PropertyReference _yearSrc;
        
//        [ReferenceType(typeof(object))]
//        [SerializeField]
//        private PropertyReference _monthSrc;
//        
//        [ReferenceType(typeof(object))]
//        [SerializeField]
//        private PropertyReference _daySrc;

        public List<int> years {
            get { return new List<int> {2019, 2020, 2021, 2022}; }
        }
        
        public List<int> months {
            get { return new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12}; }
        }
        
        public List<int> days {
            get {
                return DateTime.DaysInMonth(
                selectedYear >= years.FirstOrDefault() ? selectedYear : years.FirstOrDefault(), 
                selectedMonth >= months.FirstOrDefault() ? selectedMonth : months.FirstOrDefault()).MakeList(i => i + 1); }
        }

        public int selectedYear { get; private set; }
        
        public int selectedMonth { get; private set; }
        
        public int selectedDay { get; private set; }

        private void Start() {
            var now = DateTime.Now;
            selectedYear = now.Year;
            selectedMonth = now.Month;
            selectedDay = now.Day;
            OnDataChanged();
        }

        public void OnDateSelected() {
            var yearObj = _yearSrc.Get<object>();
            if (yearObj is int) {
                selectedYear = (int) yearObj;
            }
            
//            var monthObj = _monthSrc.Get<object>();
//            if (monthObj is int) {
//                selectedMonth = (int) monthObj;
//            }
//            
//            var dayObj = _daySrc.Get<object>();
//            if (dayObj is int) {
//                selectedDay = (int) dayObj;
//            }
        }
    }
}
