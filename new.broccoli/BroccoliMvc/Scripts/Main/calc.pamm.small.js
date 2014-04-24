function CalcSmall() {
	var jborn = 0;
	var leventa = 0;
	var hozyin = 0;
	var votfx = 0;
	var indextop20 = 0;
	var stable30 = 0;
	var investobolin = 0;
	var sven = 0;
	var otmar = 0;
	var nakopitelny = 0;

	var value = $('#small').spinner('value');

	if (value >= 10 && value < 60) {
		jborn = 100;
	}

	if (value >= 60 && value < 150) {
		jborn = 16.667;
		leventa = 83.33;
	}

	if (value >= 150 && value < 200) {
		leventa = 33.33;
		hozyin = 66.66;
	}

	if (value >= 200 && value < 300) {
		votfx = 50;
		hozyin = 50;
	}

	if (value >= 300 && value < 400) {
		votfx = 33.33;
		hozyin = 33.33;
		indextop20 = 33.33;
	}

	if (value >= 400 && value < 500) {
		stable30 = 25;
		votfx = 25;
		hozyin = 25;
		indextop20 = 25;
	}

	if (value >= 500 && value < 600) {
		stable30 = 20;
		votfx = 20;
		hozyin = 20;
		investobolin = 20;
		indextop20 = 20;
	}

	if (value >= 600 && value < 700) {
		stable30 = 16.66;
		votfx = 16.66;
		hozyin = 16.66;
		investobolin = 16.66;
		leventa = 16.66;
		indextop20 = 16.66;
	}

	if (value >= 700 && value < 900) {
		stable30 = 14.285714;
		votfx = 14.285714;
		hozyin = 14.285714;
		investobolin = 14.285714;
		leventa = 14.285714;
		jborn = 14.285714;
		indextop20 = 14.285714;
	}

	if (value >= 900 && value < 1100) {
		stable30 = 11.11;
		votfx = 11.11;
		hozyin = 11.11;
		investobolin = 11.11;
		leventa = 11.11;
		jborn = 11.11;
		indextop20 = 11.11;
		sven = 22.22;
	}

	if (value >= 1100 && value < 1300) {
		stable30 = 9.09;
		votfx = 9.09;
		hozyin = 9.09;
		investobolin = 9.09;
		leventa = 9.09;
		jborn = 9.09;
		indextop20 = 9.09;
		sven = 18.18;
	}

	if (value >= 1300 && value < 1500) {
		stable30 = 7.6923;
		votfx = 7.6923;
		hozyin = 7.6923;
		investobolin = 7.6923;
		leventa = 7.6923;
		jborn = 7.6923;
		indextop20 = 7.6923;
		sven = 15.384615;
	}

	// fxtrend
	if (jborn + leventa + hozyin + votfx + investobolin + sven + otmar > 0) {
		$('#small-fxtrend').html(Math.round(value * (jborn + leventa + hozyin + votfx + investobolin + sven + otmar) * 0.01) + '$');
	}
	else {
		$('#small-fxtrend').html('—');
	}

	if (jborn != 0) {
		$('#small-fxtrend-1').html(Math.round(value * jborn * 0.01) + '$ <span class="percent">(' + jborn.toFixed(2) + '%)</span>');
		$('#small-fxtrend-1').parent().show();
	}
	else {
		$('#small-fxtrend-1').parent().hide();
	}

	if (leventa != 0) {
		$('#small-fxtrend-2').html(Math.round(value * leventa * 0.01) + '$ <span class="percent">(' + leventa.toFixed(2) + '%)</span>');
		$('#small-fxtrend-2').parent().show();
	}
	else {
		$('#small-fxtrend-2').parent().hide();
	}

	if (hozyin != 0) {
		$('#small-fxtrend-3').html(Math.round(value * hozyin * 0.01) + '$ <span class="percent">(' + hozyin.toFixed(2) + '%)</span>');
		$('#small-fxtrend-3').parent().show();
	}
	else {
		$('#small-fxtrend-3').parent().hide();
	}

	if (votfx != 0) {
		$('#small-fxtrend-4').html(Math.round(value * votfx * 0.01) + '$ <span class="percent">(' + votfx.toFixed(2) + '%)</span>');
		$('#small-fxtrend-4').parent().show();
	}
	else {
		$('#small-fxtrend-4').parent().hide();
	}

	if (investobolin != 0) {
		$('#small-fxtrend-5').html(Math.round(value * investobolin * 0.01) + '$ <span class="percent">(' + investobolin.toFixed(2) + '%)</span>');
		$('#small-fxtrend-5').parent().show();
	}
	else {
		$('#small-fxtrend-5').parent().hide();
	}

	if (sven != 0) {
		$('#small-fxtrend-6').html(Math.round(value * sven * 0.01) + '$ <span class="percent">(' + sven.toFixed(2) + '%)</span>');
		$('#small-fxtrend-6').parent().show();
	}
	else {
		$('#small-fxtrend-6').parent().hide();
	}

	if (otmar != 0) {
		$('#small-fxtrend-7').html(Math.round(value * otmar * 0.01) + '$ <span class="percent">(' + otmar.toFixed(2) + '%)</span>');
		$('#small-fxtrend-7').parent().show();
	}
	else {
		$('#small-fxtrend-7').parent().hide();
	}

	// mmcis 
	if (indextop20 > 0) {
		$('#small-mmcis').html(Math.round(value * indextop20 * 0.01) + '$');
	}
	else {
		$('#small-mmcis').html('—');
	}

	if (indextop20 != 0) {
		$('#small-mmcis-1').html(Math.round(value * indextop20 * 0.01) + '$ <span class="percent">(' + indextop20.toFixed(2) + '%)</span>');
		$('#small-mmcis-1').parent().show();
	}
	else {
		$('#small-mmcis-1').parent().hide();
	}

	// panteon
	if (stable30 > 0) {
		$('#small-panteon').html(Math.round(value * stable30 * 0.01) + '$');
	}
	else {
		$('#small-panteon').html('—');
	}

	if (stable30 != 0) {
		$('#small-panteon-1').html(Math.round(value * stable30 * 0.01) + '$ <span class="percent">(' + stable30.toFixed(2) + '%)</span>');
		$('#small-panteon-1').parent().show();
	}
	else {
		$('#small-panteon-1').parent().hide();
	}

	// forexinn
	if (nakopitelny > 0) {
		$('#small-forexinn').html(Math.round(value * nakopitelny * 0.01) + '$');
	}
	else {
		$('#small-forexinn').html('—');
	}

	if (nakopitelny != 0) {
		$('#small-forexinn-1').html(Math.round(value * nakopitelny * 0.01) + '$ <span class="percent">(' + nakopitelny.toFixed(2) + '%)</span>');
		$('#small-forexinn-1').parent().show();
	}
	else {
		$('#small-forexinn-1').parent().hide();
	}
}