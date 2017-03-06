<div id="resultat">
<?php
// SJEKK POST
if(isset($_POST['mail'])) {
	// FILBANE TIL MAILINGLISTE
	$listfile = "mailingliste/mailingliste.txt";
	// SJEKK LENGDE
	if(@strlen($_POST['mail']) > 0) {
		// HENT VERDI
		$mail = strtolower(stripslashes($_POST['mail']));
		// SJEKK OM DET ER EN EPOST-ADRESSE
		if (preg_match("/^[^@]*@[^@]*\.[^@]*$/", $mail)) {
			// HENT EPOSTLISTE
			$list = @explode(",",file_get_contents($listfile));
			if(isset($_POST['inn'])) {
				// MELD INN
				$finnes = false; // Finnes eposten?
				// SJEKK OM MAILEN FINNES ALLEREDE
				if(count($list) >= 1) {
					foreach($list as $email) {
						if($mail == $email) {
							$finnes = true;
						}
					}
				}
				if($finnes == false) {
					// LEGG TIL EPOST I LISTE
					file_put_contents($listfile,@file_get_contents($listfile).$mail.",");
					echo "Your email address is added to our mailing list";
				} else {
					echo "This email address is already in the list.";
				}
			} elseif(isset($_POST['ut'])) {
				// MELD UT
				$finnes = false; // Finnes eposten?
				// SJEKK OM MAILEN FINNES ALLEREDE
				if(count($list) >= 1) {
					foreach($list as $email) {
						if($mail == $email) {
							$finnes = true;
						}
					}
				}
				if($finnes == true) {
					// FJERN EPOST FRA LISTE
					file_put_contents($listfile,str_replace($mail.",","",file_get_contents($listfile)));
					echo "Your email address is removed from our mailing list";
				} else {
					echo "This email address is not found in the mailing list";
				}
			} else {
				echo "An error occurred, please try again";
			}
		} else {
			echo "This is not an email address";
		}
	} else {
		echo "The email address given is too short.";
	}
}
?>
</div>
<form action="?" method="post" id="mailingliste">
	<table>
		<tr>
        	<td>E-mail:&nbsp;</td>
            <td><input id="mail" name="mail" type="text" /></td>
		</tr>
		<tr>
        	<td></td>
            <td><input type="submit" name="inn" value="Subscribe" />&nbsp;
				<input type="submit" name="ut" value="Unsubscribe" /></td>
		</tr>
	</table>
</form>