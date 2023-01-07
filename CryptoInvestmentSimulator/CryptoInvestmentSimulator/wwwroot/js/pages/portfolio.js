
// Functions to show/hide user details editing modal (pop-up).
function hideEditDetailsModal()
{
    $('#editDetailsModal').modal('hide');
}

function showEditDetailsModal()
{
    $('#editDetailsModal').modal('show');
}

// Functions to show/hide portfolio reset modal (pop-up).
function hideResetPortfolioModal()
{
    $('#resetPortfolioModal').modal('hide');
}

function showResetPortfolioModal()
{
    $('#resetPortfolioModal').modal('show');
}

// Returns true if value contains only letters.
function isOnlyLetters(str)
{
    return /^[A-Za-z]*$/.test(str);
}

// Returns true if value is between 6 to 16 characters long.
function isValidLength(str)
{
    if (str.length >= 6 && str.length <= 16)
    {
        return true;
    }

    return false;
}

// Returns true if valid url can be constructed from string.
function isValidUrl(str)
{
    try
    {
        new URL(str);
        return true;
    }
    catch (err)
    {
        return false;
    }
}

// Returns true if string contains image file type.
function isImage(str)
{
    if (str.includes('.png') || str.includes('.jpg'))
    {
        return true;
    }

    return false;
}

// Validates if portfolio reset key entered by user matches actual key.
// Doubles as protection against SQL injections.
$('#resetForm').on('submit', function ()
{
    if ($('#receivedKey').val() != $('#actualKey').val())
    {
        $('#resetError').html("Keys do not match!");
        return false;
    }
    else
    {
        $('#resetError').html("");
        hideResetPortfolioModal();
        return true;
    }
});

// Validates if entered username contains only letters and has length between 6 to 16.
// Validates if entered avatar url is a valid image url.
// Doubles as protection against SQL injections.
$('#editForm').on('submit', function ()
{
    if (!isOnlyLetters($('#username').val()))
    {
        $('#usernameError').html("Username cannot contain numbers or special symbols!");
        return false;
    }
    else if (!isValidLength($('#username').val()))
    {
        $('#usernameError').html("Username must be between 6 to 16 characters long!");
        return false;
    }
    else if (!isValidUrl($('#avatar').val()))
    {
        $('#avatarError').html("Entered image URL is not valid!");
        return false;
    }
    else if (!isImage($('#avatar').val()))
    {
        $('#avatarError').html("Entered URL is not an image!");
        return false;
    }
    else
    {
        $('#usernameError').html("");
        $('#avatarError').html("");
        hideEditDetailsModal();
        return true;
    }
});

$(document).ready(function ()
{
    // Hides all page modals by default.
    hideEditDetailsModal();
    hideResetPortfolioModal();

    // Displays modals on edit or reset click.
    document.getElementById("edit").addEventListener("click", showEditDetailsModal);
    document.getElementById("reset").addEventListener("click", showResetPortfolioModal);

    // Hides modals if cancel is clicked.
    document.getElementById("cancelEdit").addEventListener("click", hideEditDetailsModal);
    document.getElementById("cancelReset").addEventListener("click", hideResetPortfolioModal);
});

// Fills time zone dropdown with all possible time zones.
window.addEventListener('DOMContentLoaded', (event) =>
{
    var timezoneSelect = document.getElementById("time-zone-select");

    for (let i in time_zones)
    {
        let newOption = document.createElement('option');
        newOption.innerHTML = time_zones[i];
        newOption.value = time_zones[i];

        timezoneSelect.appendChild(newOption);
    }
});

// Loads _WalletTable partial view into walletTableZone div.
// Loads _InvestmentTable partial view into investmentTableZone div.
window.addEventListener("load", (event) =>
{
    $('#walletTableZone').load("/Portfolio/WalletTable");
    // setInterval(function () { $('#walletTableZone').load("/Portfolio/WalletTable"); }, 60000);

    $('#investmentTableZone').load("/Portfolio/InvestmentTable");
    // setInterval(function () { $('#investmentTableZone').load("/Portfolio/InvestmentTable"); }, 60000);
});
