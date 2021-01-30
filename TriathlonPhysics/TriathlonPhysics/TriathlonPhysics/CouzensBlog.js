
//Carb Calculator
function carbcalc() {
    var FTP = document.getElementById("FTP").value;
    var TSS = document.getElementById("TSS").value;
    var $FTP = (parseInt(FTP));
    var $TSS = (parseInt(TSS));
    if ($FTP < 240) { var CHO = 9; }
    else if ($FTP <= 200) { var CHO = 8; }
    else if ($FTP <= 240) { var CHO = 9; }
    else if ($FTP <= 270) { var CHO = 10; }
    else if ($FTP <= 300) { var CHO = 11; }
    else if ($FTP <= 330) { var CHO = 12; }
    else if ($FTP <= 360) { var CHO = 13; }
    else if ($FTP <= 390) { var CHO = 14; }
    else { var CHO = 15; }
    var Cal = $TSS * CHO;
    var Carb = Cal / 4;
    document.getElementById("Carb").value = Carb;
}

//Training Camp Recovery Calculator
$(function () {

    $("#slider1").slider({
        orientation: "vertical",
        range: "max",
        value: 100,
        min: 000,
        max: 200,
        step: 10,
        slide: function (event, ui) {
            $("#amount").val(ui.value + " %");
            if (ui.value > 1) { $("#amount2").val(200 - (ui.value) + " %"); $("#slider2").slider("option", "value", (200 - (ui.value))); }

        }
    });


    $("#slider2").slider({
        orientation: "vertical",
        range: "max",
        value: 100,
        min: 0,
        max: 200,
        step: 10,
        slide: function (event, ui2) {
            $("#amount2").val((ui2.value) + " %");
            if (ui2.value > 1) { $("#amount3").val((ui2.value) + " %"); $("#slider3").slider("option", "value", (ui2.value)); }

        }
    });

    $("#slider3").slider({
        orientation: "vertical",
        range: "max",
        value: 100,
        min: 0,
        max: 200,
        step: 10,
        slide: function (event, ui3) {
            $("#amount3").val((ui2.value) + " %");
            if (ui2.value > 1) { $("#amount3").val((ui2.value) + " %"); $("#slider3").slider("option", "value", (ui2.value)); }

        }
    });
    $("#amount").val($("#slider1").slider("value") + " %");
    $("#amount2").val($("#slider2").slider("value") + " %");
    $("#amount3").val($("#slider3").slider("value") + " %");


});

function dropcalc() {
    var hipangle = document.getElementById("hipangle").value;
    var hipangle2 = parseInt(hipangle);
    var seatangle = document.getElementById("seatangle").value;
    var seatangle2 = parseInt(seatangle);
    var torsolength = document.getElementById("torsolength").value;
    var torsolength2 = parseInt(torsolength);
    var armlength = document.getElementById("armlength").value;
    var armlength2 = parseInt(armlength);
    var trunkangle = hipangle2 - seatangle2;
    var dropdiv = Math.sin(trunkangle * (Math.PI / 180));
    var drop = Math.round(-1 * ((torsolength2 * dropdiv) - armlength2));
    document.getElementById("ta").innerHTML = trunkangle;
    document.getElementById("drop").innerHTML = drop;
    document.getElementById("drop2").value = drop;
}
function stackcalc() {
    var seatheight = document.getElementById("height").value;
    var seatheight = parseInt(seatheight);
    var drop2 = document.getElementById("drop2").value;
    var drop2 = parseInt(drop2);
    var padstack = 17;
    var padheight = (seatheight - drop2)
    var stack = (seatheight - drop2 - padstack);
    document.getElementById("padheight").innerHTML = padheight;
    document.getElementById("stack").innerHTML = stack;
}
function reachcalc() {
    var hipangle = parseInt(document.getElementById("hipangle").value);
    var seatheight = parseInt(document.getElementById("height").value);
    var seatangle = parseInt(document.getElementById("seatangle").value);
    var seatangle3 = 90 - seatangle;
    var setbackang = Math.tan(seatangle3 * (Math.PI / 180));
    var setback = Math.round(seatheight * setbackang);
    document.getElementById("setback").innerHTML = setback;
    var torsolength3 = parseInt(document.getElementById("torsolength").value);
    var armlength = parseInt(document.getElementById("armlength").value);
    var trunkangle = hipangle - seatangle;
    var cockpitang = Math.cos(trunkangle * (Math.PI / 180));
    var cockpit = Math.round(torsolength3 * cockpitang);
    document.getElementById("cockpit").innerHTML = cockpit;
    var reachhi = cockpit - setback - 5;
    var reachlo = cockpit - setback - 10;
    document.getElementById("reachlo").innerHTML = reachlo;
    document.getElementById("reachhi").innerHTML = reachhi;
}

//Swimlimiters2
$(document).ready(function () {
    $('#but').click(function () {
        var height = $('#height').val();
        var recmin = $('#recmin').val();
        var recsec = $('#recsec').val();
        var easymin = $('#easymin').val();
        var easysec = $('#easysec').val();
        var steadymin = $('#steadymin').val();
        var steadysec = $('#steadysec').val();
        var modmin = $('#modmin').val();
        var modsec = $('#modsec').val();
        var steadysc = $('#steadysc').val();
        var modsc = $('#modsc').val();
        var CP5 = $('#CP5').val();
        $.post('swimcalc2.php', { height: height, recmin: recmin, recsec: recsec, steadymin: steadymin, steadysec: steadysec, modmin: modmin, modsec: modsec, steadysc: steadysc, modsc: modsc, CP5: CP5 }, function (data, status) {
            $('#txtHint').html(data);
        });
    });
});

//Crank length calculator
function crankcalc() {
    var hipangleb = parseInt(document.getElementById("hipangleb").value);
    var seatheightb = parseInt(document.getElementById("seatheightb").value);
    var seatangleb = parseInt(document.getElementById("seatangleb").value);
    var seatanglec = 90 - seatangleb;
    var closedhip = parseInt(document.getElementById("closedhip").value);
    var virttib = parseInt(document.getElementById("virttib").value);
    var femur = parseInt(document.getElementById("femur").value);
    var diff = hipangleb - closedhip;
    var totalside = Math.sin(Math.PI / 180 * (diff)) * (seatheightb / Math.sin(Math.PI / 180 * (180 - diff - seatanglec)));
    var crank = Math.ceil(((totalside - virttib) * 10) / 2.5) * 2.5;
    document.getElementById("crank").innerHTML = crank;
}

//Bike Split Calculator
function BikeSplitCalc() {
    var weight = parseInt($('#weight').val());
    var power = parseInt($('#power').val());
    var race = $('#race').val();
    var relpow = power / weight;
    if (race == 'Kona') {
        var split = Math.round(9.52 * Math.pow(relpow, -0.53) * 100) / 100;
        $('#split').html(split);
    }
    else if (race == 'AZ') {
        var split = Math.round(8.82 * Math.pow(relpow, -0.53) * 100) / 100;
        $('#split').html(split);
    }
    else if (race == 'FL') {
        var split = Math.round(8.83 * Math.pow(relpow, -0.53) * 100) / 100;
        $('#split').html(split);
    }
    else if (race == 'LP') {
        var split = Math.round(9.45 * Math.pow(relpow, -0.50) * 100) / 100;
        $('#split').html(split);
    }
    else if (race == 'COZ') {
        var split = Math.round(11.53 * Math.pow(relpow, -0.70) * 100) / 100;
        $('#split').html(split);
    }
    else if (race == 'Boulder') {
        var split = Math.round(10.0 * Math.pow(relpow, -0.62) * 100) / 100;
        $('#split').html(split);
    }
    else if (race == 'CDA') {
        var split = Math.round(9.3 * Math.pow(relpow, -0.5) * 100) / 100;
        $('#split').html(split);
    }
}
