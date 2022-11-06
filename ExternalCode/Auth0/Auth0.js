
// Extra scripts added in html document
// <script src="https://cdn.auth0.com/js/auth0/9.18/auth0.min.js"></script>
// <script src="https://cdn.auth0.com/js/polyfills/1.0/object-assign.min.js"></script>
  
window.addEventListener('load', function() {

	var config = JSON.parse(decodeURIComponent(escape(window.atob('@@config@@'))));
	var leeway = config.internalOptions.leeway;
	
	if (leeway) 
	{
		var convertedLeeway = parseInt(leeway);
		
		if (!isNaN(convertedLeeway)) 
		{
			config.internalOptions.leeway = convertedLeeway;
		}
	}

	var params = Object.assign({
		overrides: {
			__tenant: config.auth0Tenant,
			__token_issuer: config.authorizationServer.issuer
		},
		domain: config.auth0Domain,
		clientID: config.clientID,
		redirectUri: config.callbackURL,
		responseType: 'code'
	}, config.internalOptions);

	var webAuth = new auth0.WebAuth(params);
	var databaseConnection = 'Username-Password-Authentication';
	var captcha = webAuth.renderCaptcha(document.querySelector('.captcha-container'));

	function login(e) 
	{
		e.preventDefault();
		
		var button = this;
		var username = document.getElementById('email').value;
		var password = document.getElementById('password').value;
		
		button.disabled = true;
		webAuth.login({
			realm: databaseConnection,
			username: username,
			password: password,
			captcha: captcha.getValue()
		}, function(err) {
			if (err) displayError(err);
			button.disabled = false;
		});
	}

	function signup() 
	{
		var button = this;
		var given_name = document.getElementById('given_name').value;
		var family_name = document.getElementById('family_name').value;
		var email = document.getElementById('email').value;
		var password = document.getElementById('password').value;

		button.disabled = true;
		webAuth.redirect.signupAndLogin({
			connection: databaseConnection,
			given_name: given_name,
			family_name: family_name,
			email: email,
			password: password,
			captcha: captcha.getValue()
		}, function(err) {
			if (err) displayError(err);
			button.disabled = false;
		});
	}

	function loginWithGoogle() 
	{
		webAuth.authorize({
			connection: 'google-oauth2'
		}, function(err) {
			if (err) displayError(err);
		});
	}

	function displayError(err) 
	{
		captcha.reload();
		
		var errorMessage = document.getElementById('error-message');
		errorMessage.innerHTML = err.policy || err.description;
		errorMessage.style.display = 'block';
	}
		  
	function showExtra() 
	{
		var name = document.getElementById("name-field");
		var surname = document.getElementById("surname-field");
		var login = document.getElementById("btn-login");
		var google = document.getElementById("btn-google");
		var signup = document.getElementById("btn-signup");
		var switchLogin = document.getElementById("switch-to-login");
		var switchSignup = document.getElementById("switch-to-signup");
				
		name.style.display = "block";
		surname.style.display = "block";
		signup.style.display = "block";
		switchLogin.style.display = "block";
				
		login.style.display = "none";
		google.style.display = "none";
		switchSignup.style.display = "none";
	}
		  
	function hideExtra() 
	{
		var name = document.getElementById("name-field");
		var surname = document.getElementById("surname-field");
		var login = document.getElementById("btn-login");
		var google = document.getElementById("btn-google");
		var signup = document.getElementById("btn-signup");
		var switchLogin = document.getElementById("switch-to-login");
		var switchSignup = document.getElementById("switch-to-signup");
				
		name.style.display = "none";
		surname.style.display = "none";
		signup.style.display = "none";
		switchLogin.style.display = "none";
				
		login.style.display = "block";
		google.style.display = "block";
		switchSignup.style.display = "block";
	}

	document.getElementById('btn-login').addEventListener('click', login);
	document.getElementById('btn-google').addEventListener('click', loginWithGoogle);
	document.getElementById('btn-signup').addEventListener('click', signup);
	document.getElementById('switch-to-login').addEventListener('click', hideExtra);
	document.getElementById('switch-to-signup').addEventListener('click', showExtra);
});