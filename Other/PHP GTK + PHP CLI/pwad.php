<?php
/*
* dl() is deprecated in PHP 5 - use php.ini to load PHP-GTK 2
*/
if (!class_exists('gtk')) {
    die('Please load the php-gtk2 module in your php.ini' . "\r\n");
}

class adpwd extends GtkWindow
{
	private $txt = array(
		"username"	=> "Username:",
		"password"	=> "Password:",
		"login"		=> "Login");
	// Buttons
	private	$btnLogin;
	//private	$btnCancel;
	// Labels
	private $lblUsername;
	private $lblPassword;
	// Textfields
	private $txtUsername;
	private $txtPassword;
    function __construct()
    {
        parent::__construct();
		$this->connect_simple('destroy', array('gtk', 'main_quit'));
        $this->set_title('Sett passord');
        //$this->set_default_size(400, 250);
		
		$this->set_position(Gtk::WIN_POS_CENTER);
		
		// Textfields
		$this->txtUsername = new GtkEntry();
		$this->txtPassword = new GtkEntry();
		$this->txtPassword->set_visibility(false); // Create password field
		// Create labels
		$this->lblUsername = new GtkLabel($this->txt['username'], true);
		$this->lblPassword = new GtkLabel($this->txt['password'], true);
		// Set labels to textfields
		$this->lblUsername->set_mnemonic_widget($this->txtPassword);
		$this->lblPassword->set_mnemonic_widget($this->txtUsername);
		
		// Create buttons
		$this->btnLogin = new GtkButton('_Login');
		//$this->btnCancel = new GtkButton('_Cancel');
		// Create events
		$this->btnLogin ->connect_simple('clicked', array($this,'login'),$this);
		//$this->btnCancel->connect_simple('clicked', array($this, 'destroy'));
		
		// Create table
		$tbl = new GtkTable(3, 2);
		//$tbl->attach($lblCredit, 0, 2, 0, 1);
		$tbl->attach($this->lblUsername, 0, 1, 0, 1);
		$tbl->attach($this->txtUsername, 1, 2, 0, 1);
		$tbl->attach($this->lblPassword, 0, 1, 1, 2);
		$tbl->attach($this->txtPassword, 1, 2, 1, 2);
		
		// Create buttonbox
		$bbox = new GtkHButtonBox();
		$bbox->set_layout(Gtk::BUTTONBOX_EDGE);
		//$bbox->add($this->btnCancel);
		$bbox->add($this->btnLogin);
		
		// Create design
		$vbox = new GtkVBox();
		$vbox->pack_start($tbl);
		$vbox->pack_start($bbox);
		
		$this->add($vbox);
		$this->show_all();
		
    }//function __construct()
	public function alert(GtkWindow $wnd, $msg, $title = __CLASS__, $type = Gtk::MESSAGE_INFO, $dialog = Gtk::DIALOG_MODAL, $buttons = Gtk::BUTTONS_OK) {
		// SHOWS A DIALOG BOX
		
		/* OPTIONS */
		/*
		$type >
		
		0	Gtk::MESSAGE_INFO	Informational message
		1	Gtk::MESSAGE_WARNING	Nonfatal warning message
		2	Gtk::MESSAGE_QUESTION	Question requiring a choice
		3	Gtk::MESSAGE_ERROR	Fatal error message 
		
		$dialog >
			
		1	Gtk::DIALOG_MODAL	Make the constructed dialog modal, see set_modal.
		2	Gtk::DIALOG_DESTROY_WITH_PARENT	Destroy the dialog when its parent is destroyed, see set_destroy_with_parent() .
		4	Gtk::DIALOG_NO_SEPARATOR	Don't put a separator between the action area and the dialog content. 
		
		$buttons >
		
		0	Gtk::BUTTONS_NONE	No buttons at all. (use add_buttons()) - http://gtk.php.net/manual/en/gtk.enum.buttonstype.php
		1	Gtk::BUTTONS_OK	An OK button.
		2	Gtk::BUTTONS_CLOSE	A Close button.
		3	Gtk::BUTTONS_CANCEL	A Cancel button.
		4	Gtk::BUTTONS_YES_NO	Yes and No buttons.
 		5	Gtk::BUTTONS_OK_CANCEL	OK and Cancel buttons. 
		*/
		
		
		if($msg == "") {
			$msg = "\$msg is null";
		}
		$dialog = new GtkMessageDialog($wnd, $dialog, $type, $buttons, $title);
        $dialog->set_markup($msg);
        $dialog->run();
        $dialog->destroy();
	}
	function login() {
		$pid = $this->txtUsername->get_text();
    	$credential = $this->txtPassword->get_text();
		if(strlen($pid) > 0 and strlen($credential) > 0) {
    		//ldap will bind anonymously, make sure we have some credentials//
			$ldap = ldap_connect("ldaps://vibe.iris.local",636) or die(ldap_error());
			$baseDn = "o=RF";
			ldap_set_option($ldap, LDAP_OPT_PROTOCOL_VERSION, 3);
			#ldap_set_option($ldap, LDAP_OPT_REFERRALS, 0);
			if (isset($ldap) && $ldap != '') {
				// search for pid dn //
				$result = @ldap_search($ldap, $baseDn, 'uid='.$pid, array('dn'));
				#print_r($result);
				if ($result != 0) {
					$entries = ldap_get_entries($ldap, $result);
					$principal = $entries[0]['dn'];
					if (isset($principal)) {
						// bind as this user //
						if (@ldap_bind($ldap, $principal, $credential)) {
							echo ";NXDAYN;AUTHDT:".md5($pid.$credential);
						} else {
							$error = ";NXDAYN;AUTHERL6";
						}
					} else {
						$error = ";NXDAYN;AUTHERL5";
					}
					ldap_free_result($result);
				} else {
					$error = ";NXDAYN;AUTHERL4";
				}
				ldap_close($ldap);
			} else {
				$error = ";NXDAYN;AUTHERL3";
			}
			if($error != "") {
				$this->alert($this,"hei");
			}
		} else {
			$this->alert($this,"Some of the fields are empty");
		}
		return false;
	}
}//class PHPGtk2Demo extends GtkWindow

$GLOBALS['framework'] = true;
new adpwd();
Gtk::main();
?>
