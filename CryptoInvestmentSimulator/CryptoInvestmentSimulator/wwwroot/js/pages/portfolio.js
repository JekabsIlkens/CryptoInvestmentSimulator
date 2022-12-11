
// Functions to show/hide user details editing popup.
function hideEditDetailsModal()
{
    $('#exampleModalCenter').modal('hide');
}

function showEditDetailsModal()
{
    $('#exampleModalCenter').modal('show');
}

$(document).ready(function ()
{
    hideEditDetailsModal();

    document.getElementById("edit").addEventListener("click", showEditDetailsModal);
    document.getElementById("cancel").addEventListener("click", hideEditDetailsModal);
    document.getElementById("save").addEventListener("click", hideEditDetailsModal);
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
