
window.addEventListener('DOMContentLoaded', (event) => {
	var leverageSelect = document.getElementById("leverage-ratio-select");

	for (let i in leverage_ratios) {
		let newOption = document.createElement('option');
		newOption.innerHTML = leverage_ratios[i];
		newOption.value = leverage_ratios[i];

		leverageSelect.appendChild(newOption);
	}

	var fiatAmountInput = document.getElementById("receivedEuro");
	var cryptoAmountInput = document.getElementById("convertedCrypto");

	fiatAmountInput.addEventListener("input", function (e) {
		var fiatAmountString = "" + fiatAmountInput.value;
		var unitValueString = "" + (priceArray1h[priceArray1h.length - 1]);

		var unitValueParsed = parseFloat(unitValueString);
		var fiatAmountParsed = parseFloat(fiatAmountString);

		var cryptoAmountResult = "" + (unitValueParsed * fiatAmountParsed);

		cryptoAmountInput.value = cryptoAmountResult;
	});
});
