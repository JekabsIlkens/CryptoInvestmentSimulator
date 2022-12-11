
// Loads all cardano charts, adds a refresh interval and switch buttons
window.addEventListener("load", (event) => {
	$('#chartZone1h').load("/Market/ADA1hChart");
	setInterval(function () { $('#chartZone1h').load("/Market/ADA1hChart"); }, 60000);
	document.getElementById("chartZone1h").style.display = "block";

	$('#chartZone4h').load("/Market/ADA4hChart");
	setInterval(function () { $('#chartZone4h').load("/Market/ADA4hChart"); }, 60250);
	document.getElementById("chartZone4h").style.display = "none";

	$('#chartZone8h').load("/Market/ADA8hChart");
	setInterval(function () { $('#chartZone8h').load("/Market/ADA8hChart"); }, 60500);
	document.getElementById("chartZone8h").style.display = "none";

	$('#chartZone24h').load("/Market/ADA24hChart");
	setInterval(function () { $('#chartZone24h').load("/Market/ADA24hChart"); }, 60750);
	document.getElementById("chartZone24h").style.display = "none";

	document.getElementById("1h").addEventListener("click", get1hChart);
	document.getElementById("4h").addEventListener("click", get4hChart);
	document.getElementById("8h").addEventListener("click", get8hChart);
	document.getElementById("24h").addEventListener("click", get24hChart);
});
