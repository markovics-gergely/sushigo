# Sushi Go Party!

## Specifikáció

### Funkciók

#### Felhasználó

- Felhasználók beléptetése, regisztrálása
- Cookie használata
- Játék alatt szerzett pontok kezelése
- Beváltott játékcsomagok kezelése
- Játékelőzmények megjelenítése

#### Barátok

- Felhasználók bejelölése
- elküldött és fogadott barátkérések megjelenítése
- Barátkérések élőben kezelése
- Barátok online-offline státuszának megjelenítése

#### Bolt

- Minden felhasználó alap Classic joggal kezd
- Party jog beváltása pontokból
- Játékcsomagok kezelése
- Csomagok beváltása pontokból
- Oldal megjelenítése a csomagokból adott jog alapján

#### Váróterem

- Várótermek létrehozása
- Tulajdonos-létrehozó felhasználó
- Tulajdonos választja a használt csomagot
- Minden felhasználónak meg kell lennie a kiválasztott csomagnak
- Csomaghoz illő létszámnak kell lennie a váróteremben
- Bárki behívhat barátot a terembe
- Meghívó élőben elküldése
- Chat

#### Játék

- TBA ősszel

### Technológiák, tervezési minták

#### Szerver

- Mikroszolgáltatások
  - Ocelot
    - Közös swagger dev felület
  - Docker-compose
  - .NET 7
  - user, lobby, store felosztás
- Adatbázis
  - MSSQL (EntityFramework)
  - Redis (lekérdezés cache-elés)
- IdentityServer
  - Jwt aktív felparaméterezése
- AutoMapper
- ProblemDetails
- SignalR
  - Chat, barátok, meghívók
- Mediatr
  - Pipeline-ok (cache, log)
  - SignalR összehangolás
  - UnitOfWork + Repository minta (db hozzáférés)
  - Validator minta
- RabbitMq

#### Kliens

- Angular 15
- Angular Material
- többnyelvűség (i18n)
- SignalR
- Cookie token
- Interceptor
- Acl minta (guard + directive)
- Overlayek
  - Töltés alatt
  - Barátkezelés
- Reszponzív felület
  - Mobilon és kisebb képernyőn is használható
- dotenv (secret kezelés)

### Játékleírás

**[Játékszabályzat](https://tarsasjatekok.com/files/common/f/f5/f54/f5489c4e84b252e77f45f8a12895022b/sushi-go-party-szabaly-lowres.pdf)**  

Egy nagy buli sushi tálat kell összeraknotok! De maradjon ám hely a desszertnek is!
Szerezz minél több pontot az ételek kártyák ügyes kiválogatásával! A Sushi Go Party!-ban csak a játékosokon múlik, milyen ételek kerülnek terítékre! A játék elején összeállított menühöz tartozó kártyákból a játékosszámnak megfelelőt kell kiosztani. Minden kártya másképpen ér pontot, de a kapott lapok közül mindig csak egy választható - a többit tovább kell adni a szomszédnak. Jól gondold meg, mit választasz!

Tofut ennél, megkívántad a miso levest, vagy innál egy finom teát? Esetleg a klasszikus fogásokhoz ragaszkodsz? Minden étel másképp ér pontot, de minden körben csak egyet választhatsz - a többit tovább kell adnod a szomszédnak. A Sushi Go Party! változatában saját ízlésednek megfelelően állíthatod össze a menüt, így jól gondold meg, mit veszel le a futószalagról! Az eredeti Sushi Go Party! "örökölte" az eredeti Sushi Go! játékmenetét, de rengeteg új fogással egészítette ki, így végeredményül egy hatalmas pakknyi partyhangulatot kaptunk.

A játékosok a játék elején kiválaszthatják, milyen ételfajtákkal szeretnének játszani, az ezekhez tartozó lapkákat felrakják a menüre, emlékeztetőül. A kártyákat össze kell keverni, majd a játékosszámnak megfelelő mennyiségű kártyát kell kiosztani. A játékosok kézbe veszik a kapott lapokat, kiválasztanak egyet, a többit pedig továbbadják. A kiválasztott kártyát fel kell fordítani, majd a kapott lapok közül a játékosok ismét kiválasztanak egyet. Ez egészen addig folytatódik, ameddig minden lap le nem került. Aki a harmadik ilyen forduló után (tehát mikor a harmadik kiosztott pakli kártyát is lehelyezték a játékosok) a legtöbb ponttal rendelkezik, az nyert.

Mivel minden étel más és más módon ér pontot, így kombinációik miatt a játék mindig változatos és mindig más stratégiát kíván meg. A játékszabály a játékosszámtól és a kívánt nehézségi szinttől függően javasol étel-összeállításokat, de a tapasztaltabb játékosok saját menüt is összeállíthatnak.
