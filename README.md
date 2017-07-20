# PhoneBook

<p>The normal Bulgarian phone number is like: +359878123456, where:</p>
<ul>
<li>+359 is the code for Bulgarian numbers;</li>
<li>The next 2 digits are the code of the operator, where the 3 operators have their own combinations- 87, 88, 89;</li>
<li>The next digit is in range from 2 to 9;</li>
<li>The last 6 digits are in range from 0 to 9.<li>
</ul>

<p>Equivalents to +359878123456 are:</p>
<ul>
<li>0878123456, where 0 replaces +359;</li>
<li>00359878123456, where 00 replaces +;</li>
</ul>

<p>All other combinations of symbols are invalid for Bulgarian phone numbers.</p>

<p>White a class PhoneBook, which models a phonebook and contains pairs (name, normal phone number). PhoneBook has functions:</p>
<ul>
<li>Constructs new phonebook from a text file – in each row of the text document there is a of pair of kind(name, number), where name is a random name and the number is a phone number. When reading the text file, concern that some of the pairs may be invalid. Ignore the invalid pairs and load only the valid ones. In PhoneBook , all phone numbers should be saved in normalized form;</li>
<li>Add a new pair;</li>
<li>Delete a pair by name;</li>
<li>Access to phone number by name;</li>
<li>Print all pairs sorted by name.</li>
</u>

<p><b>By realizing the class, take into account that the most used operation will be Print.</b></p>

<p>Realize additional functionality, which holds the number of outgoing calls from phone book. In effective way must print on the screen, sorted five pairs from phonebook, which are registered with the biggest count of registered outgoing calls.</p>
