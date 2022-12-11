
// Functions for switching charts in view
function get1hChart() {
	document.getElementById("chartZone1h").style.display = "block";
	document.getElementById("chartZone4h").style.display = "none";
	document.getElementById("chartZone8h").style.display = "none";
	document.getElementById("chartZone24h").style.display = "none";
}

function get4hChart() {
	document.getElementById("chartZone1h").style.display = "none";
	document.getElementById("chartZone4h").style.display = "block";
	document.getElementById("chartZone8h").style.display = "none";
	document.getElementById("chartZone24h").style.display = "none";
}

function get8hChart() {
	document.getElementById("chartZone1h").style.display = "none";
	document.getElementById("chartZone4h").style.display = "none";
	document.getElementById("chartZone8h").style.display = "block";
	document.getElementById("chartZone24h").style.display = "none";
}

function get24hChart() {
	document.getElementById("chartZone1h").style.display = "none";
	document.getElementById("chartZone4h").style.display = "none";
	document.getElementById("chartZone8h").style.display = "none";
	document.getElementById("chartZone24h").style.display = "block";
}
