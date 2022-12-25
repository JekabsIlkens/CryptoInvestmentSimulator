
// Functions to show/hide user details editing popup.
function hideEditDetailsModal()
{
    $('#exampleModalCenter').modal('hide');
}

function showEditDetailsModal()
{
    $('#exampleModalCenter').modal('show');
}

// Functions to show/hide portfolio reset popup.
function hideResetPortfolioModal() {
    $('#exampleModalCenter2').modal('hide');
}

function showResetPortfolioModal() {
    $('#exampleModalCenter2').modal('show');
}

$(document).ready(function ()
{
    hideEditDetailsModal();
    hideResetPortfolioModal();

    document.getElementById("edit").addEventListener("click", showEditDetailsModal);
    document.getElementById("reset").addEventListener("click", showResetPortfolioModal);

    document.getElementById("cancelEdit").addEventListener("click", hideEditDetailsModal);
    document.getElementById("cancelReset").addEventListener("click", hideResetPortfolioModal);

    document.getElementById("save").addEventListener("click", hideEditDetailsModal);
    document.getElementById("confirm").addEventListener("click", hideResetPortfolioModal);
});

window.addEventListener('DOMContentLoaded', (event) =>
{
    var timezoneSelect = document.getElementById("time-zone-select");

    for (let i in time_zones) {
        let newOption = document.createElement('option');
        newOption.innerHTML = time_zones[i];
        newOption.value = time_zones[i];

        timezoneSelect.appendChild(newOption);
    }
});
