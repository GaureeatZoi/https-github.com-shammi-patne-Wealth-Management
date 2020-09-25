// 65-90 => A-Z
// 97-122 => a-z
// 32 => SPACE
// 8 => BACKSPACE
// 127 => DEL
// 44 - COMMA(,)
// 45 - HYPEN (-)
// 46 - DOT (.)
// 47 - Backslash(/)



//      Example :   AmarRamamoorthy
function AlphabetsOnly(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122) || keyGet == 127 || keyGet == 8)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   8098754568
function NumbersOnly(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if ((keyGet >= 48 && keyGet <= 57) || keyGet == 127 || keyGet == 8)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   AMAR
function UpperCaseOnly(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }   
    if ((keyGet >= 65 && keyGet <= 90) || keyGet == 127 || keyGet == 8)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   amar
function LowerCaseOnly(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if ((keyGet >= 97 && keyGet <= 122) || keyGet == 127 || keyGet == 8)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   31/17 Koil Street, LNS-Puram,Eruvadi.
function AlphabetsWithSpaceWithSpecial(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122) || keyGet == 32 || keyGet == 34
        || (keyGet >= 48 && keyGet <= 57)
        || keyGet == 127 || keyGet == 8 || (keyGet >= 44 && keyGet <= 47))
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")  
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :  amarramamoorthy@gmail.com 
function Email(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }
    if ((keyGet >= 97 && keyGet <= 122)  || keyGet == 127 || keyGet == 46 ||keyGet == 64 || keyGet == 8 || (keyGet >= 48 && keyGet <= 57))
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        window.event.returnValue = null;
    }
    else {
        evtGet.preventDefault();
    }
}

//      Example :  Amar-Zoifintech 
function AlphabetsWithoutSpaceWithHyphen(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122))
        return;
    else if (keyGet == 8 || keyGet == 127 || keyGet == 45)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   Amar113
function AlphaNumeric(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if (keyGet > 47 && keyGet < 58)
        return;    
    else if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122))
        return;
    else if (keyGet == 8 ||  keyGet == 127)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   Amar Ramamoorthy
function AlphabetsWithSpace(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }
    if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122))
        return;   
    else if (keyGet == 8 || keyGet == 0 || keyGet == 127 || keyGet == 32)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   AMAR - 12345
function AlphaNumericWithHypenWithSpace(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if (keyGet > 47 && keyGet < 58)
        return;
    else if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122))
        return;    
    else if (keyGet == 8 ||  keyGet == 127 || keyGet == 32 || keyGet == 45)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}

//      Example :   AMAR-12345
function AlphaNumericWithHypenWithoutSpace(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    if (keyGet > 47 && keyGet < 58)
        return;
    else if ((keyGet >= 65 && keyGet <= 90) || (keyGet >= 97 && keyGet <= 122))
        return;    
    else if (keyGet == 8 ||  keyGet == 127  || keyGet == 45)
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox")
        window.event.returnValue = null;
    else
        evtGet.preventDefault();
}


//--  Onchange Events --\\

//      Example :   THIYA1234A
function PANCard(evtGet) {
    var browser = navigator.appName;
    if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        var keyGet = evtGet.keyCode;
    }
    else {
        var keyGet = evtGet.which;
    }

    var regex = /([A-Z]){5}([0-9]){4}([A-Z]){1}$/;
    var char = String.fromCharCode(keyGet);
    var isValid = regex.test(char);    
    if (isValid)
        window.event.returnValue = keyGet;

    if ((keyGet >= 65 && keyGet <= 90) || keyGet == 127 || keyGet == 8 || (keyGet >= 48 && keyGet <= 57))
        return;
    else if (browser == "Microsoft Internet Explorer" || browser == "Mozilla Firefox") {
        window.event.returnValue = null;
    }
    else {
        evtGet.preventDefault();
    }


}