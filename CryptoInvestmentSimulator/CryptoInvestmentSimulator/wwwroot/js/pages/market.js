
// Loads market data table as partial view in designated tableZone div.
// Sets a refresh interval for dynamic table updates when new data is collected.

window.addEventListener("load", (event) =>
{
	$('#tableZone').load("/Market/DataTable");
	setInterval(function () { $('#tableZone').load("/Market/DataTable"); }, 60000);
});
