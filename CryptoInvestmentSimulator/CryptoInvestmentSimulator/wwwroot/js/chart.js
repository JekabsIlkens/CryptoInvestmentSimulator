
// Creates and configures a renderable price chart.
function getChart(crypto, history)
{
	var chart = new CanvasJS.Chart("chartContainer",
	{
		zoomEnabled: false,
		axisX:
		{
			title: "",
			crosshair:
			{
				enabled: true,
				snapToDataPoint: true
			}
		},
		axisY:
		{
			crosshair:
			{
				enabled: true,
				snapToDataPoint: true,
				valueFormatString: "#,###0"
			}
		},
		toolTip:
		{
			shared: true
		},
		legend:
		{
			dockInsidePlotArea: true,
			verticalAlign: "top",
			horizontalAlign: "right"
		},
		data:
		[{
			type: "line",
			markerType: "none",
			xValueType: "dateTime",
			xValueFormatString: "hh:mm:ss",
			name: crypto,
			showInLegend: true,
			dataPoints: history
		}]
	});

	return chart;
}
