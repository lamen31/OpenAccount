let monthVal;
let startDate;
let endDate;

var todayDate = new Date();
var yesterdayDate = new Date(todayDate.getFullYear(), todayDate.getMonth(), todayDate.getDate() - 1);
var tomorrrowDate = new Date(todayDate.getFullYear(), todayDate.getMonth(), todayDate.getDate() + 1);
var minLimit = new Date(todayDate.getFullYear() - 2, todayDate.getMonth(), todayDate.getDate() + 1);
var minLimit2 = yesterdayDate;
var maxLimit2 = todayDate;
var startDatePicker = false;

function setDatePicker() {
	try {
		initDateTimePicker()
		$(".startdate").datetimepicker({
			format: "DD/MM/YYYY",
			useCurrent: false,
			minDate: minLimit,
			maxDate: yesterdayDate
		});

		$(".startdate").on("change.datetimepicker", function (e) {
			minLimit2 = new Date(moment(e.date).year(), moment(e.date).month(), moment(e.date).date() + 1);
			maxLimit2 = new Date(minLimit2.getFullYear(), minLimit2.getMonth() + 1, minLimit2.getDate() - 2);
			if (maxLimit2 < todayDate) {
				//console.log("Max Limit Less Than Today:" + minLimit2 + " " + maxLimit2);
			} else {
				maxLimit2 = todayDate;
				//console.log("Max Limit More Than Today:" + minLimit2 + " " + maxLimit2);
			}
			$(".enddate").datetimepicker('minDate', minLimit2);
			$(".enddate").datetimepicker('format', "DD/MM/YYYY");
			$(".enddate").datetimepicker('maxDate', maxLimit2);
			$(".enddate").val("");
			//console.log("Update Data StartDate:" + minLimit2 + maxLimit2 + e.date);
		});

		$(".enddate").datetimepicker('minDate', minLimit2);
		$(".enddate").datetimepicker('format', "DD/MM/YYYY");
		$(".enddate").datetimepicker('maxDate', maxLimit2);
		$(".enddate").val("");

		$(".enddate").on("change.datetimepicker", function (e) {
			//console.log("Update Data EndDate:" + e.date);
			//console.log("Later Date Picker");
		});

	} catch (err) {
		//console.log(err)
    }
}

function initDateTimePicker() {
	$(".datepicker").datetimepicker({
		format: "YYYY-MM-DD",
		useCurrent: false
	})
}

function setEndDatePicker() {

}

function setDateRangePicker(input1, input2){
	$(input1).datetimepicker({
		format: "DD/MM/YYYY",
		useCurrent: false
	})

	$(input1).on("change.datetimepicker", function (e) {
		$(input2).val("")
        $(input2).datetimepicker('minDate', e.date);
    })

	$(input2).datetimepicker({
		format: "DD/MM/YYYY",
		useCurrent: false
	})
}

function setMonthPicker(){
	$(".monthpicker").datetimepicker({
		locale: '@System.Threading.Thread.CurrentThread.CurrentUICulture.Name',
		format: "MMMM-YYYY",
		useCurrent: true,
		viewMode: "months",
		minDate: minLimit,
		maxDate: new Date()
	})
}

function getStartDatePicker() {
	try {
		//console.log("Start Date : " + moment($(".startdate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY'));
		return moment($(".startdate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY');
	}
	catch(err) {
		return ""
    }
}

function getEndDatePicker() {
	
	try {
		//console.log("End Date : " + moment($(".enddate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY'));
		return moment($(".enddate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY');
	}
	catch (err) {
		return ""
	}
}

function getMonthPicker() {

	try {
		//console.log("Month Range : " + moment($(".monthpicker").data("datetimepicker")._dates[0]).format('DD/MM/YYYY') + "#" + moment($(".monthpicker").data("datetimepicker")._dates[0]).endOf('month').format('DD/MM/YYYY'));
		return moment($(".monthpicker").data("datetimepicker")._dates[0]).format('DD/MM/YYYY') + "#" + moment($(".monthpicker").data("datetimepicker")._dates[0]).endOf('month').format('DD/MM/YYYY');
	}
	catch (err) {
		return ""
	}
}

function getMonthValue() {
	return monthVal;
}
function getStartDateValue() {
	return startDate;
}
function getEndDateValue() {
	return endDate;
}

function initializeDatePickerComponent(container) {
	var $monthPicker = $(container.querySelector('#monthpicker'));
	var $dateStart = $(container.querySelector('#startdate'));
	var $dateEnd = $(container.querySelector('#enddate'));
	$monthPicker.datetimepicker({
		format: "MMMM-YYYY",
		useCurrent: true,
		viewMode: "months"
	})
	$dateStart.datetimepicker({
		format: "YYYY-MM-DD",
		useCurrent: false
	})
	$dateEnd.datetimepicker({
		format: "YYYY-MM-DD",
		useCurrent: false
	})
}

