function AddSubtractMonths(date, numOfMonths) {
    //month will be a value from 0 (jan) to 11 (dec)
    var month = date.getMonth();
    //The result will be in milliseconds
    var milliseconds = new Date(date).setMonth(month + numOfMonths);
    //This will return a date which is more or less than the date provided depending on the noOfMonths value
    return new Date(milliseconds);
}