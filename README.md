# Booking
Booking.bak on MS SQL serveri andmebaasi varukoopia.

BookingBackend on veebiteenus Booking rakendusele.
Booking on töölauarakendus hotelli töötajale.
BookingsWeb on veebileht kliendile.

BookingBackend ja BookingsWeb on seadistatud kasutama andmebaasi kohalikust arvutist kohaliku kasutaja õigustes.
Kui andmebaas asub mujal või nõuab erinevaid õiguseid, on vaja muuta vastavalt connection stringid web.config failides.

Booking rakendus vajab kasutamiseks BookingBackend veebiteenust.
BookingBackend töötab Visual Studiost käivitades aadressil https://localhost:44306/BookingService.asmx
Kui teenus on installitud kuhugi mujale, on vaja vastavalt muuta endpointi aadress Booking rakenduse app.config failis.

Booking rakendus käivitub tühja aknaga, vasakul all nuppudest saab avada vajalikud moodulid. 
Topeltklikk tabelis avab vastava kirje muutmiseks või kustutamiseks. Märgitud kirje saab avada ka  nupust 'Ava...'.
Kustutada ei saa kliente ja tubasid, mis on seotud broneeringuga.
Kustutada saab broneeringuid, mille algus on hilisem kui kolmas päev.

BookingsWeb kasutajatunnuseks on kliendi meiliaadress.
Avalehel on nimekiri kliendi broneeringutest koos broneeringu kustutamise võimalusega.
Uue broneeringu puhul on vaja esmalt määrata broneeringu algus- ja lõpuaeg ning soovitud voodite arv.
Nupp 'Otsi tube' kuvab nimekirja valitud perioodi sobivatest tubadest.
Nupp 'Alusta uuesti' annab võimaluse muuta broneeringu aega või voodite arvu.

Probleemide ja küsimuste korral tel 5287589 või urmas@magi.ee
