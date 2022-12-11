
// Refreshes market page data table when new data gets collected.
window.addEventListener("load", (event) => {
	$('#tableZone').load("/Market/DataTable");
	setInterval(function () { $('#tableZone').load("/Market/DataTable"); }, 60000);
});
