var NAMESHOP;var TT;var coun;var g = new Array();var m = new Array();var f = new Array();var k1;var k2;var k3;var k4;var k5;var k6;var k7;var k8;var k9;var k10;var k11;var k12;var k13;var k14;var k15;var k16;var k17;var k18;var p1;var p2;var p3;var p4;var p5;var p6;var p7;var p8;var p9;var p10;var p11;var p12;var p13;var p14;var p15;var p16;var p17;var p18;function checkinput(evt) {if (!evt) evt = event;if (evt.charCode) {var charCode = evt.charCode; } else if (evt.keyCode) {var charCode = evt.keyCode; }else if (evt.which) { var charCode = evt.which;}else {var charCode = 0;}if (charCode > 31 && (charCode < 48 || charCode > 57 || charCode == 46)) {if (charCode == 46 || charCode == 46) {return true;}else { return false; }}return true;}
function calcf() { var a1 = this.anketa.TextBox4s_konk0.value; var a2 = this.anketa.TextBox4s_konk1.value; var a3 = this.anketa.TextBox4s_konk2.value; var a4 = this.anketa.TextBox4s_konk3.value; var a5 = this.anketa.TextBox4s_konk4.value; var a6 = this.anketa.TextBox4s_konk5.value; var a7 = this.anketa.TextBox4s_konk6.value; var a8 = this.anketa.TextBox4s_konk7.value; var a9 = this.anketa.TextBox4s_konk8.value; var a10 = this.anketa.TextBox4s_konk9.value; var a11 = this.anketa.TextBox4s_konk10.value; var a12 = this.anketa.TextBox4s_konk11.value; var a13 = this.anketa.TextBox4s_konk12.value; var a14 = this.anketa.TextBox4s_konk13.value; var a15 = this.anketa.TextBox4s_konk14.value; var a16 = this.anketa.TextBox4s_konk15.value; var a17 = this.anketa.TextBox4s_konk16.value; var a18 = this.anketa.TextBox4s_konk17.value; g[1] = a1; g[2] = a2; g[3] = a3; g[4] = a4; g[5] = a5; g[6] = a6; g[7] = a7; g[8] = a8; g[9] = a9; g[10] = a10; g[11] = a11; g[12] = a12; g[13] = a13; g[14] = a14; g[15] = a15;g[16] = a16;g[17] = a17;g[18] = a18;if (a1 == "") { this.anketa.TextBox4s_konk0.value = 0; } if (a2 == "") { this.anketa.TextBox4s_konk1.value = 0; }if (a3 == "") { this.anketa.TextBox4s_konk2.value = 0; } if (a4 == "") { this.anketa.TextBox4s_konk3.value = 0; } if (a5 == "") { this.anketa.TextBox4s_konk4.value = 0; } if (a6 == "") { this.anketa.TextBox4s_konk5.value = 0; } if (a7 == "") { this.anketa.TextBox4s_konk6.value = 0; } if (a8 == "") { this.anketa.TextBox4s_konk7.value = 0; } if (a9 == "") { this.anketa.TextBox4s_konk8.value = 0; } if (a10 == "") { this.anketa.TextBox4s_konk9.value = 0; } if (a11 == "") { this.anketa.TextBox4s_konk10.value = 0; } if (a12 == "") { this.anketa.TextBox4s_konk11.value = 0; } if (a13 == "") { this.anketa.TextBox4s_konk12.value = 0; } if (a14 == "") { this.anketa.TextBox4s_konk13.value = 0; } if (a15 == "") { this.anketa.TextBox4s_konk14.value = 0; } if (a16 == "") { this.anketa.TextBox4s_konk15.value = 0; } if (a17 == "") { this.anketa.TextBox4s_konk16.value = 0; } if (a18 == "") { this.anketa.TextBox4s_konk17.value = 0; }}
function checkinput2(evt) { if (!evt) evt = event; if (evt.charCode) { var charCode = evt.charCode; } else if (evt.keyCode) { var charCode = evt.keyCode; } else if (evt.which) { var charCode = evt.which; } else { var charCode = 0;}if (charCode > 31 && (charCode < 48 || charCode > 57 || charCode == 44)) { return false; } return true;}
function che(e) { var a1 = this.anketa.TextBox4s_konk0.value; var a2 = this.anketa.TextBox4s_konk1.value; var a3 = this.anketa.TextBox4s_konk2.value; var a4 = this.anketa.TextBox4s_konk3.value; var a5 = this.anketa.TextBox4s_konk4.value; var a6 = this.anketa.TextBox4s_konk5.value; var a7 = this.anketa.TextBox4s_konk6.value; var a8 = this.anketa.TextBox4s_konk7.value; var a9 = this.anketa.TextBox4s_konk8.value; var a10 = this.anketa.TextBox4s_konk9.value; var a11 = this.anketa.TextBox4s_konk10.value; var a12 = this.anketa.TextBox4s_konk11.value; var a13 = this.anketa.TextBox4s_konk12.value; var a14 = this.anketa.TextBox4s_konk13.value; var a15 = this.anketa.TextBox4s_konk14.value; var a16 = this.anketa.TextBox4s_konk15.value; var a17 = this.anketa.TextBox4s_konk16.value; var a18 = this.anketa.TextBox4s_konk17.value; var d = 0; d = parseFloat(a1) + parseFloat(a2) + parseFloat(a3) + parseFloat(a4) + parseFloat(a5) + parseFloat(a6) + parseFloat(a7) + parseFloat(a8) + parseFloat(a9) + parseFloat(a10) + parseFloat(a11) + parseFloat(a12) + parseFloat(a13) + parseFloat(a14) + parseFloat(a15) + parseFloat(a16) + parseFloat(a17) + parseFloat(a18); this.anketa.TextBox4s_konk54.value = d; calcall()}
function che2(e) { var a1 = this.anketa.TextBox4s_konk18.value;  var a2 = this.anketa.TextBox4s_konk36.value; var a3 = this.anketa.TextBox4s_konk37.value; var a4 = this.anketa.TextBox4s_konk38.value; var a5 = this.anketa.TextBox4s_konk39.value; var a6 = this.anketa.TextBox4s_konk40.value;  var a7 = this.anketa.TextBox4s_konk41.value; var a8 = this.anketa.TextBox4s_konk42.value; var a9 = this.anketa.TextBox4s_konk43.value; var a10 = this.anketa.TextBox4s_konk44.value; var a11 = this.anketa.TextBox4s_konk45.value; var a12 = this.anketa.TextBox4s_konk46.value; var a13 = this.anketa.TextBox4s_konk47.value; var a14 = this.anketa.TextBox4s_konk48.value; var a15 = this.anketa.TextBox4s_konk49.value;var a16 = this.anketa.TextBox4s_konk50.value;var a17 = this.anketa.TextBox4s_konk51.value;var a18 = this.anketa.TextBox4s_konk51.value;m[1] = a1; m[2] = a2; m[3] = a3; m[4] = a4; m[5] = a5; m[6] = a6; m[7] = a7; m[8] = a8; m[9] = a9; m[10] = a10; m[11] = a11; m[12] = a12; m[13] = a13; m[14] = a14; m[15] = a15; m[16] = a16; m[17] = a17; m[18] = a18; calcall()}
function calcf2() { var a1 = this.anketa.TextBox4s_konk18.value; var a2 = this.anketa.TextBox4s_konk36.value;  var a3 = this.anketa.TextBox4s_konk37.value; var a4 = this.anketa.TextBox4s_konk38.value; var a5 = this.anketa.TextBox4s_konk39.value; var a6 = this.anketa.TextBox4s_konk40.value; var a7 = this.anketa.TextBox4s_konk41.value; var a8 = this.anketa.TextBox4s_konk42.value; var a9 = this.anketa.TextBox4s_konk43.value; var a10 = this.anketa.TextBox4s_konk44.value;  var a11 = this.anketa.TextBox4s_konk45.value; var a12 = this.anketa.TextBox4s_konk46.value; var a13 = this.anketa.TextBox4s_konk47.value;  var a14 = this.anketa.TextBox4s_konk48.value; var a15 = this.anketa.TextBox4s_konk49.value; var a16 = this.anketa.TextBox4s_konk50.value; var a17 = this.anketa.TextBox4s_konk51.value; var a18 = this.anketa.TextBox4s_konk51.value;if (a1 == "") { this.anketa.TextBox4s_konk18.value = "0"; } if (a2 == "") { this.anketa.TextBox4s_konk36.value = "0"; } if (a3 == "") { this.anketa.TextBox4s_konk37.value = "0"; } if (a4 == "") { this.anketa.TextBox4s_konk38.value = "0"; } if (a5 == "") { this.anketa.TextBox4s_konk39.value = "0"; } if (a6 == "") { this.anketa.TextBox4s_konk40.value = "0"; } if (a7 == "") { this.anketa.TextBox4s_konk41.value = "0"; } if (a8 == "") { this.anketa.TextBox4s_konk42.value = "0"; } if (a9 == "") { this.anketa.TextBox4s_konk43.value = "0"; } if (a10 == "") { this.anketa.TextBox4s_konk44.value = "0"; } if (a11 == "") { this.anketa.TextBox4s_konk45.value = "0"; } if (a12 == "") { this.anketa.TextBox4s_konk46.value = "0"; } if (a13 == "") { this.anketa.TextBox4s_konk47.value = "0"; } if (a14 == "") { this.anketa.TextBox4s_konk48.value = "0"; } if (a15 == "") { this.anketa.TextBox4s_konk49.value = "0"; } if (a16 == "") { this.anketa.TextBox4s_konk50.value = "0"; } if (a17 == "") { this.anketa.TextBox4s_konk51.value = "0"; } if (a18 == "") { this.anketa.TextBox4s_konk52.value = "0"; }}
function calcf3() { var a1 = this.anketa.Text1.value; var a2 = this.anketa.Text2.value; var a3 = this.anketa.Text3.value; var a4 = this.anketa.Text4.value; var a5 = this.anketa.Text5.value; var a6 = this.anketa.Text6.value; var a7 = this.anketa.Text7.value;  var a8 = this.anketa.Text8.value; var a9 = this.anketa.Text9.value; var a10 = this.anketa.Text10.value;  var a11 = this.anketa.Text11.value; var a12 = this.anketa.Text12.value; var a13 = this.anketa.Text13.value; var a14 = this.anketa.Text14.value; var a15 = this.anketa.Text15.value; var a16 = this.anketa.Text16.value; var a17 = this.anketa.Text17.value; var a18 = this.anketa.Text18.value; coun = 0;if (a1 == "") { this.anketa.Text1.style.borderColor = '#CFCFCF'; } else { this.anketa.Text1.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a2 == "") { this.anketa.Text2.style.borderColor = '#CFCFCF'; } else { this.anketa.Text2.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a3 == "") { this.anketa.Text3.style.borderColor = '#CFCFCF'; } else { this.anketa.Text3.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a4 == "") { this.anketa.Text4.style.borderColor = '#CFCFCF'; } else { this.anketa.Text4.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a5 == "") { this.anketa.Text5.style.borderColor = '#CFCFCF'; } else { this.anketa.Text5.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a6 == "") { this.anketa.Text6.style.borderColor = '#CFCFCF'; } else { this.anketa.Text6.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a7 == "") { this.anketa.Text7.style.borderColor = '#CFCFCF'; } else { this.anketa.Text7.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a8 == "") { this.anketa.Text8.style.borderColor = '#CFCFCF'; } else { this.anketa.Text8.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a9 == "") { this.anketa.Text9.style.borderColor = '#CFCFCF'; } else { this.anketa.Text9.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a10 == "") { this.anketa.Text10.style.borderColor = '#CFCFCF'; } else { this.anketa.Text10.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a11 == "") { this.anketa.Text11.style.borderColor = '#CFCFCF'; } else { this.anketa.Text11.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a12 == "") { this.anketa.Text12.style.borderColor = '#CFCFCF'; } else { this.anketa.Text12.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a13 == "") { this.anketa.Text13.style.borderColor = '#CFCFCF'; } else { this.anketa.Text13.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a14 == "") { this.anketa.Text14.style.borderColor = '#CFCFCF'; } else { this.anketa.Text14.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a15 == "") { this.anketa.Text15.style.borderColor = '#CFCFCF'; } else { this.anketa.Text15.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a16 == "") { this.anketa.Text16.style.borderColor = '#CFCFCF'; } else { this.anketa.Text16.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a17 == "") { this.anketa.Text17.style.borderColor = '#CFCFCF'; } else { this.anketa.Text17.style.borderColor = '#00C1F6'; coun = coun + 1; } if (a18 == "") { this.anketa.Text18.style.borderColor = '#CFCFCF'; } else { this.anketa.Text18.style.borderColor = '#00C1F6'; coun = coun + 1; }f[1] = a1; f[2] = a2; f[3] = a3; f[4] = a4; f[5] = a5; f[6] = a6; f[7] = a7; f[8] = a8; f[9] = a9; f[10] = a10; f[11] = a11; f[12] = a12; f[13] = a13; f[14] = a14; f[15] = a15; f[16] = a16; f[17] = a17; f[18] = a18;}function proverka() { var vg; var par = readPar(); var d1 = this.anketa.TextBox4s_konk.value;

 vg = "?&idTpl=" + TT; 
vg = vg + "&TTName=" + NAMESHOP;
 vg =  vg + "&typeank=2";
vg = vg + "&Order=" + d1; vg = vg + "&TCount=" + coun; var r = 0; for (i = 1; i <= 18; i++) { if (f[i] != "") {  r = r + 1; vg = vg + "&TID_" + r + "="; vg = vg + "&TC_" + r + "=" + g[i]; vg = vg + "&TPr_" + r + "=" + m[i]; vg = vg + "&TName_" + r + "=" + encodeURIComponent(f[i]);  } }var vb = "https://anketa.bank.rs.ru/minipotreb_d.php" + vg;location.href = "" + vb + "";}
function redr() {if (this.anketa.Text1.value == "") { }  else { this.anketa.Text2.readOnly = false; } if (this.anketa.Text2.value == "") { this.anketa.Text3.readOnly = true;  } else { this.anketa.Text3.readOnly = false; }if (this.anketa.Text3.value == "") { this.anketa.Text4.readOnly = true; } else { this.anketa.Text4.readOnly = false; } if (this.anketa.Text4.value == "") { this.anketa.Text5.readOnly = true; } else { this.anketa.Text5.readOnly = false; } if (this.anketa.Text5.value == "") { this.anketa.Text6.readOnly = true; } else { this.anketa.Text6.readOnly = false; } if (this.anketa.Text6.value == "") { this.anketa.Text7.readOnly = true; } else { this.anketa.Text7.readOnly = false; } if (this.anketa.Text4.value == "") { this.anketa.Text8.readOnly = true; } else { this.anketa.Text8.readOnly = false; } if (this.anketa.Text8.value == "") { this.anketa.Text9.readOnly = true; } else { this.anketa.Text9.readOnly = false; } if (this.anketa.Text9.value == "") { this.anketa.Text10.readOnly = true; } else { this.anketa.Text10.readOnly = false; } if (this.anketa.Text10.value == "") { this.anketa.Text11.readOnly = true; } else { this.anketa.Text11.readOnly = false; } if (this.anketa.Text11.value == "") { this.anketa.Text12.readOnly = true; } else { this.anketa.Text12.readOnly = false; } if (this.anketa.Text12.value == "") { this.anketa.Text13.readOnly = true; } else { this.anketa.Text13.readOnly = false; } if (this.anketa.Text13.value == "") { this.anketa.Text14.readOnly = true; } else { this.anketa.Text14.readOnly = false; } if (this.anketa.Text14.value == "") { this.anketa.Text15.readOnly = true; } else { this.anketa.Text15.readOnly = false; } if (this.anketa.Text15.value == "") { this.anketa.Text16.readOnly = true; } else { this.anketa.Text16.readOnly = false; } if (this.anketa.Text16.value == "") { this.anketa.Text17.readOnly = true; } else { this.anketa.Text17.readOnly = false; } if (this.anketa.Text17.value == "") { this.anketa.Text18.readOnly = true; } else { this.anketa.Text18.readOnly = false; }}
function calcall() { k1 = this.anketa.TextBox4s_konk0.value; k2 = this.anketa.TextBox4s_konk1.value; k3 = this.anketa.TextBox4s_konk2.value; k4 = this.anketa.TextBox4s_konk3.value; k5 = this.anketa.TextBox4s_konk4.value; k6 = this.anketa.TextBox4s_konk5.value; k7 = this.anketa.TextBox4s_konk6.value; k8 = this.anketa.TextBox4s_konk7.value; k9 = this.anketa.TextBox4s_konk8.value; k10 = this.anketa.TextBox4s_konk9.value; k11 = this.anketa.TextBox4s_konk10.value; k12 = this.anketa.TextBox4s_konk11.value; k13 = this.anketa.TextBox4s_konk12.value; k14 = this.anketa.TextBox4s_konk13.value; k15 = this.anketa.TextBox4s_konk14.value; k16 = this.anketa.TextBox4s_konk15.value; k17 = this.anketa.TextBox4s_konk16.value; k18 = this.anketa.TextBox4s_konk17.value; p1 = this.anketa.TextBox4s_konk18.value; p2 = this.anketa.TextBox4s_konk36.value; p3 = this.anketa.TextBox4s_konk37.value; p4 = this.anketa.TextBox4s_konk38.value; p5 = this.anketa.TextBox4s_konk39.value; p6 = this.anketa.TextBox4s_konk40.value; p7 = this.anketa.TextBox4s_konk41.value;  p8 = this.anketa.TextBox4s_konk42.value; p9 = this.anketa.TextBox4s_konk43.value; p10 = this.anketa.TextBox4s_konk44.value; p11 = this.anketa.TextBox4s_konk45.value; p12 = this.anketa.TextBox4s_konk46.value; p13 = this.anketa.TextBox4s_konk47.value; p14 = this.anketa.TextBox4s_konk48.value; p15 = this.anketa.TextBox4s_konk49.value; p16 = this.anketa.TextBox4s_konk50.value; p17 = this.anketa.TextBox4s_konk51.value;p18 = this.anketa.TextBox4s_konk51.value;var d = 0;d = parseFloat(p1) * parseFloat(k1) + parseFloat(p2) * parseFloat(k2) + parseFloat(p3) * parseFloat(k3) + parseFloat(p4) * parseFloat(k4) + parseFloat(p5) * parseFloat(k5) + parseFloat(p6) * parseFloat(k6) + parseFloat(p7) * parseFloat(k7) + parseFloat(p8) * parseFloat(k8) + parseFloat(p9) * parseFloat(k9) + parseFloat(p10) * parseFloat(k10) + parseFloat(p11) * parseFloat(k11) + parseFloat(p12) * parseFloat(k12) + parseFloat(p13) * parseFloat(k13) + parseFloat(p14) * parseFloat(k14) + parseFloat(p15) * parseFloat(k15) + parseFloat(p16) * parseFloat(k16) + parseFloat(p17) * parseFloat(k17) + parseFloat(p18) * parseFloat(k18);this.anketa.TextBox4s_konk53.value = d;}function readPar() { var par = new Object(); var a = document.location.search.substr(1).split('&'); var parItem; var i; for (i = 0; i < a.length; i++) { parItem = a[i].split('='); par[parItem[0]] = parItem[1]; } return par; }function ttxx() {if (NAMESHOP != "") {ttd.innerHTML = "Оформление заказа в кредит<br /><br />  <small>Магазин: " + NAMESHOP + "</small>";}}
