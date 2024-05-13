$(function () {

    const addMonths = 2;
    var currDate = new Date();
    function WireUpDatePicker() {
        $('.datepicker').datepicker(
            {
                dateFormat: 'yy-mm-dd',
                minDate: currDate,
                maxDate: AddSubtractMonths(currDate, addMonths)
            }
        );
    }
    WireUpDatePicker();
   
});