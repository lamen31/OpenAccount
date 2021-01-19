let monthVal;
let startDate;
let endDate;

var todayDate = new Date();
var minLimit = new Date(todayDate.getFullYear() - 2, todayDate.getMonth(), todayDate.getDate())
var yesterdayDate = new Date(todayDate.getFullYear(), todayDate.getMonth(), todayDate.getDate() - 1)
var tomorrrowDate = new Date(todayDate.getFullYear(), todayDate.getMonth(), todayDate.getDate() + 1)
var minLimit2 = new Date();

function setDatePicker() {
	try {
		$(".startdate").datetimepicker({
			locale: '@System.Threading.Thread.CurrentThread.CurrentUICulture.Name',
			format: "DD/MM/YYYY",
			useCurrent: false,
			minDate: minLimit,
			maxDate: yesterdayDate
		})
		$(".startdate").on("change.datetimepicker", function (e) {
			
			minLimit2 = new Date(moment(e.date).year(), moment(e.date).month(), moment(e.date).date() + 1);
			console.log("E Date: " + minLimit2);
			$(".enddate").val("")
			$(".enddate").datetimepicker({
				locale: '@System.Threading.Thread.CurrentThread.CurrentUICulture.Name',
				format: "DD/MM/YYYY",
				useCurrent: false,
				minDate: minLimit2,
				maxDate: todayDate
			});
			
		})
	} catch (err) {
		//console.log(err)
    }

/*	$(".enddate").datetimepicker({
		locale: '@System.Threading.Thread.CurrentThread.CurrentUICulture.Name',
		format: "DD/MM/YYYY",
		useCurrent: false
	})*/
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
		maxDate: new Date()
	})
}

function getStartDatePicker() {
	//$(".startdate").on("change.datetimepicker", function (e) {
	//	console.log(moment(e.date).format('DD/MM/YYYY'));
	//	startDate = moment(e.date).format('DD/MM/YYYY');
		//document.getElementById("startdate").value = moment(e.date).format('YYYY-MM-DD');
	//})
	//return startDate;
	try {
		//console.log("Start Date : " + moment($(".startdate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY'));
		return moment($(".startdate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY');
	}
	catch(err) {
		return ""
    }
	
	
	//return moment($(".startdate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY');
}

function getEndDatePicker() {
	//$(".enddate").on("change.datetimepicker", function (e) {
	//	console.log(moment(e.date).format('DD/MM/YYYY'));
	//	endDate = moment(e.date).format('DD/MM/YYYY');
		//document.getElementById("enddate").value = moment(e.date).format('YYYY-MM-DD');
	//})
	//return endDate;
	
	try {
		//console.log("End Date : " + moment($(".enddate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY'));
		return moment($(".enddate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY');
	}
	catch (err) {
		return ""
	}
	
	//return moment($(".enddate").data("datetimepicker")._dates[0]).format('DD/MM/YYYY');
}

function getMonthPicker() {
	//$(".monthpicker").on("change.datetimepicker", function (e) {
	//console.log(moment(e.date).format('MMMM-YYYY'));
	
	try {
		//console.log("Today: " + todayDate)
		//console.log("Before: " + minLimit)
		//console.log("Yesterday: " + yesterdayDate)
		//console.log("Tomorrow: " + tomorrrowDate)
		console.log("Month Range : " + moment($(".monthpicker").data("datetimepicker")._dates[0]).format('DD/MM/YYYY') + "#" + moment($(".monthpicker").data("datetimepicker")._dates[0]).endOf('month').format('DD/MM/YYYY'));
		return moment($(".monthpicker").data("datetimepicker")._dates[0]).format('DD/MM/YYYY') + "#" + moment($(".monthpicker").data("datetimepicker")._dates[0]).endOf('month').format('DD/MM/YYYY');
	}
	catch (err) {
		return ""
	}
	//	startDate = moment(e.date).format('DD/MM/YYYY');
	//	endDate = moment(e.date).endOf('month').format('DD/MM/YYYY')
		//console.log(monthVal);
		//document.getElementById("monthpicker").value = moment(e.date).format('MMMM-YYYY');
	//})
	//return monthVal;
	/*console.log(moment($(".monthpicker").data("datetimepicker")._dates[0]).format('MMMM-YYYY'))*/
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

