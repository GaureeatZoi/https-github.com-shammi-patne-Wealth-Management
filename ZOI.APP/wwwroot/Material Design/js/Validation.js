
// JavaScript source code

function AllowAlpha() {

    var a = event.keyCode;
    if ((a >= 65 && a <= 90) || a==46 ||(a >= 97 && a <= 122) || a == 32) {
        return true;
    }
    else {
        event.preventDefault();
    }
}

function AllowAlphaNumeric() {

    var a = event.keyCode;
    if ((a >= 65 && a <= 90) || (a >= 97 && a <= 122) || a == 46 || a == 32) {
        return true;
    }
    else {
        event.preventDefault();
    }
}

function Numeric() {

    var a = event.keyCode;
    if ((a >= 48 && a <= 57) || a == 46) {
        return true;
    }
    else {
        event.preventDefault();
    }
}

function AlphaOnly() {

    var a = event.keyCode;
    if ((a >= 65 && a <= 90) || (a >= 97 && a <= 122) || a == 46) {
        return true;
    }
    else {
        event.preventDefault();
    }
}
function MobileNumber() {

    var a = event.keyCode;
    if ((a >= 48 && a <= 57) || a == 46) {
        return true;
    }
    else {
        event.preventDefault();
    }
}

//Amar

function IFSCCode(pan) {
    var regex = /[A-Z]{5}[0-9]{4}[A-Z]{1}$/;
    return regex.test(pan);
}

//function () {
    
//    var regExp = /[A-z]{5}\d{4}[a-zA-Z]{1}/;
//    var txtpan = $(this).val();
//    if (txtpan.length < 10) {
//        if (txtpan.match(regExp))
//        {
//            event.preventDefault();
//        }
//    } else { event.preventDefault(); }
//}

