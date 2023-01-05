
window.addEventListener('DOMContentLoaded', (event) => {

	// Collects the dropdown select as a global variable.
	var leverageSelect = document.getElementById("leverage-ratio-select");

	// Fills select with all possible leverage ratio options found in ../js/leverage_ratios.js
	for (let i in leverage_ratios) {
		let newOption = document.createElement('option');
		newOption.innerHTML = leverage_ratios[i];
		newOption.value = leverage_ratios[i];

		leverageSelect.appendChild(newOption);
	}

	// Collects form input fields as global variables.
	var fiatAmountInput = document.getElementById("receivedEuro");
	var cryptoAmountInput = document.getElementById("convertedCrypto");
	var leverageRatioInput = document.getElementById("leverage-ratio-select");
	var calculatedMarginInput = document.getElementById("calculatedMargin");

	// Listens for when user changes euro amount input value.
	// Recalculates necessary crypto amount and margin.
	// Fills readonly crypto amount and margin input fields.
	fiatAmountInput.addEventListener("input", function (e)
	{
		// Collects user input from euro amount input and parses it for use in calculations.
		// Collects latest unit price from chart and parses it for use in calculations.
		var fiatAmountString = "" + fiatAmountInput.value;

		if (fiatAmountString != "") {
			var unitValueString = "" + (priceArray1h[priceArray1h.length - 1]);
			var fiatAmountParsed = parseFloat(fiatAmountString);
			var unitValueParsed = parseFloat(unitValueString);

			// Calculates equivelant amount of crypto to users input in euro.
			// Fills readonly crypto amount field.
			var cryptoAmountResult = "" + (fiatAmountParsed / unitValueParsed);
			cryptoAmountInput.value = cryptoAmountResult;

			// Each leverage ratio uses different devisor, sets calculated result as readonly margin input value.
			if (("" + leverageRatioInput.value) == "1x") { calculatedMarginInput.value = "" + (0); }

			if (("" + leverageRatioInput.value) == "2x") { calculatedMarginInput.value = "" + (fiatAmountParsed / 2); }

			if (("" + leverageRatioInput.value) == "5x") { calculatedMarginInput.value = "" + (fiatAmountParsed / 5); }

			if (("" + leverageRatioInput.value) == "10x") { calculatedMarginInput.value = "" + (fiatAmountParsed / 10); }
		}
		else
		{
			cryptoAmountInput.value = "" + 0;
			calculatedMarginInput.value = "" + 0;
		}
	});

	// Listens when a new leverage ratio dropdown option gets selected.
	// Recalculates necessary margin and fills readonly margin input field.
	leverageRatioInput.addEventListener("change", function (e)
	{
		// Collects user input from euro amount input and parses it for use in calculations.
		var fiatAmountString = "" + fiatAmountInput.value;

		if (fiatAmountString != "")
		{
			var fiatAmountParsed = parseFloat(fiatAmountString);

			// Each leverage ratio uses different devisor, sets calculated result as readonly margin input value.
			if (("" + leverageRatioInput.value) == "1x") { calculatedMarginInput.value = "" + (0); }

			if (("" + leverageRatioInput.value) == "2x") { calculatedMarginInput.value = "" + (fiatAmountParsed / 2); }

			if (("" + leverageRatioInput.value) == "5x") { calculatedMarginInput.value = "" + (fiatAmountParsed / 5); }

			if (("" + leverageRatioInput.value) == "10x") { calculatedMarginInput.value = "" + (fiatAmountParsed / 10); }
		}
		else {
			cryptoAmountInput.value = "" + 0;
			calculatedMarginInput.value = "" + 0;
		}
	});
});
