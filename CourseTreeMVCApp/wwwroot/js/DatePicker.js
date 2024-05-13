$(function () {
    $('.datepicker').datepicker(
        {
            dateFormat: 'yy-mm-dd',
            minDate: new Date(),
            maxDate: AddSubtractMonths(new Date(),2)
        }
    );
   
});