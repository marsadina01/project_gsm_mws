$('#LoginButton').attr("disabled", "disabled");

function encryptString(str) {
	let encrypted = '';
	for (let i = 0; i < str.length; i++) {
		const charCode = str.charCodeAt(i) + 1;
		encrypted += String.fromCharCode(charCode);
	}
	$('#txtPassword').val(encrypted)
	return encrypted;
}

const passwordInput = document.getElementById('txtPassword');
const plaintext = passwordInput.value;

$('#LoginButton').click(function () {
	encryptString($('#txtPassword').val())
});


$(document).ready(function () {
	$('#code').on('change keyup', function () {
		const ans = captcha.valid($('input[name="code"]').val());
		if (ans === false) {
			$('#errorCap').html('Code captcha yang anda masukkan salah!');
			$('#correctCap').html('');
			$('#LoginButton').attr("disabled", "disabled");
		} else {
			$('#errorCap').html('');
			$('#correctCap').html('Code captcha yang anda masukkan benar');
			$('#LoginButton').removeAttr("disabled");
		}
	});
	$('#checkCap').on('click', function () {
		const ans = captcha.valid($('input[name="code"]').val());
		if (ans === false) {
			$('#errorCap').html('Code captcha yang anda masukkan salah!');
			$('#correctCap').html('');
			$('#LoginButton').attr("disabled", "disabled");
			//captcha.refresh();
		} else {
			$('#errorCap').html('');
			$('#correctCap').html('Code captcha yang anda masukkan benar');
			$('#LoginButton').removeAttr("disabled");
		}
	});
	$('#valid').on('click', function () {
		captcha.refresh();
	});

	const captcha = new Captcha($('#canvas'), {
		length: 6,
		autoRefresh: false,
		caseSensitive: true,
		width: 100,
		height: 40,
		font: 'bold 23px Arial',
		resourceType: 'aA0', // a-lowercase letters, A-uppercase letter, 0-numbers
		resourceExtra: [],
		clickRefresh: true,
	});

});