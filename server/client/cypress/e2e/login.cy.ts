const uuid = () => Cypress._.random(0, 1e6);
const testname = `Test_Name${uuid()}`;

const uuid2 = () => Cypress._.random(0, 1e6);
const testname2 = `Test_Name${uuid()}`;

const lobbyName = `Test_Lobby${uuid()}`;

describe('login', () => {
  it('register', () => {
    cy.visit('/register');
    cy.get('#userName').type(testname).should('have.value', testname);
    cy.get('#firstNameField').click();
    cy.get('#firstName').type(testname).should('have.value', testname);
    cy.get('#lastNameField').click();
    cy.get('#lastName').type(testname).should('have.value', testname);
    cy.get('#emailField').click();
    cy.get('#email').type(testname + '@admin.hu').should('have.value', testname + '@admin.hu');
    cy.get('#passwordField').click();
    cy.get('#password').type(testname).should('have.value', testname);
    cy.get('#confirmedPasswordField').click();
    cy.get('#confirmedPassword').type(testname).should('have.value', testname);
    cy.get('#registerButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/login');

    
    cy.get('#username').type(testname).should('have.value', testname);
    cy.get('#password').type(testname).should('have.value', testname);
    cy.get('#loginButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
  });

  it('register again', () => {
    cy.visit('/register');
    cy.get('#userName').type(testname).should('have.value', testname);
    cy.get('#firstNameField').click();
    cy.get('#firstName').type(testname).should('have.value', testname);
    cy.get('#lastNameField').click();
    cy.get('#lastName').type(testname).should('have.value', testname);
    cy.get('#emailField').click();
    cy.get('#email').type(testname + '@admin.hu').should('have.value', testname + '@admin.hu');
    cy.get('#passwordField').click();
    cy.get('#password').type(testname).should('have.value', testname);
    cy.get('#confirmedPasswordField').click();
    cy.get('#confirmedPassword').type(testname).should('have.value', testname);
    cy.get('#registerButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/register');
    cy.get('.mat-mdc-snack-bar-label').should('contain', 'Username or e-mail is already taken (400)');
  });

  it('register another', () => {
    cy.visit('/register');
    cy.get('#userName').type(testname2).should('have.value', testname2);
    cy.get('#firstNameField').click();
    cy.get('#firstName').type(testname2).should('have.value', testname2);
    cy.get('#lastNameField').click();
    cy.get('#lastName').type(testname2).should('have.value', testname2);
    cy.get('#emailField').click();
    cy.get('#email').type(testname2 + '@admin.hu').should('have.value', testname2 + '@admin.hu');
    cy.get('#passwordField').click();
    cy.get('#password').type(testname2).should('have.value', testname2);
    cy.get('#confirmedPasswordField').click();
    cy.get('#confirmedPassword').type(testname2).should('have.value', testname2);
    cy.get('#registerButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/login');

    
    cy.get('#username').type(testname2).should('have.value', testname2);
    cy.get('#password').type(testname2).should('have.value', testname2);
    cy.get('#loginButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname2);
  });

  it('login', () => {
    cy.visit('/login');
    cy.get('#username').type(testname).should('have.value', testname);
    cy.get('#password').type(testname).should('have.value', testname);
    cy.get('#loginButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
  });

  it('login another', () => {
    cy.visit('/login');
    cy.get('#username').type(testname2).should('have.value', testname2);
    cy.get('#password').type(testname2).should('have.value', testname2);
    cy.get('#loginButton').click();
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname2);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname2);
  });

  it('logged in before', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
  });

  it('send friend request', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('#friend-button').click();
    cy.get('#friend-add-button').click();
    cy.get('#add-friend-input').type(testname2).should('have.value', testname2);
    cy.get('#add-friend-input-button').click();
    cy.get('#sent-list > mat-expansion-panel-header #mat-badge-content-1').should('have.text', '1');
    cy.get('#sent-list > mat-expansion-panel-header').click();
    cy.get('#sent-list > mat-expansion-panel-header #mat-badge-content-1').should('have.text', '0');
    cy.get('.friend-name').should('contain', testname2);

    
    cy.login(testname2, testname2);
    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname2);
    cy.get('#friend-button').click();
    cy.get('#received-list > mat-expansion-panel-header #mat-badge-content-1').should('have.length', 0);
    cy.get('#received-list > mat-expansion-panel-header').click();
    cy.get('.friend-name').should('contain', testname);

    cy.get('.accept-button').trigger("click");
    cy.get('#friend-list > mat-expansion-panel-header #mat-badge-content-0').should('have.text', '1');
    cy.get('#friend-list > mat-expansion-panel-header').click();
    cy.get('.friend-name').should('contain', testname);

    cy.login(testname, testname);
    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('#friend-button').click();
    cy.get('#friend-list > mat-expansion-panel-header #mat-badge-content-0').should('have.text', '0');
    cy.get('#friend-list > mat-expansion-panel-header').click();
    cy.get('.friend-name').should('contain', testname2);
  });

  it('send friend request again', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('#friend-button').click();
    cy.get('#friend-add-button').click();
    cy.get('#add-friend-input').type(testname2).should('have.value', testname2);
    cy.get('#add-friend-input-button').click();
    cy.get('.mat-mdc-snack-bar-label').should('contain', 'Invalid friend request (400)');
  });

  it('send friend request to yourself', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('#friend-button').click();
    cy.get('#friend-add-button').click();
    cy.get('#add-friend-input').type(testname).should('have.value', testname);
    cy.get('#add-friend-input-button').click();
    cy.get('.mat-mdc-snack-bar-label').should('contain', 'Invalid friend request (400)');
  });

  it('send friend request to non-existent user', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('#friend-button').click();
    cy.get('#friend-add-button').click();
    cy.get('#add-friend-input').type('non-existent').should('have.value', 'non-existent');
    cy.get('#add-friend-input-button').click();
    cy.get('.mat-mdc-snack-bar-label').should('contain', 'Invalid friend request (400)');
  });

  it('remove friend', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('#friend-button').click();
    cy.get('#friend-list > mat-expansion-panel-header #mat-badge-content-0').should('have.text', '0');
    cy.get('.friend-name').should('contain', testname2);

    cy.get('.decline-button').trigger("click");
    cy.get('.friend-name').should('not.exist');

    cy.login(testname2, testname2);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname2);
    cy.get('#friend-button').click();
    cy.get('#friend-list > mat-expansion-panel-header #mat-badge-content-0').should('have.text', '0');
    cy.get('.friend-name').should('not.exist');
  });

  it('create lobby', () => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');

    cy.visit('/lobby');
    cy.get('#create-lobby').click();
    cy.get('#create-lobby-form #name').type(lobbyName).should('have.value', lobbyName);
    cy.get('#create-lobby-form #password').type(lobbyName).should('have.value', lobbyName);
    cy.get('#lobby-create-button').click();
    cy.wait(500);
    cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');
  });

  after(() => {
    cy.login(testname, testname);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname);
    cy.get('.settings-buttons .open-icon').trigger('click');
    cy.get('#settings-delete-user').trigger('click');
    cy.get('#confirm-accept').trigger('click');
    cy.wait(500);
    cy.location('pathname', { timeout: 5000 }).should('contain', '/login');

    cy.login(testname2, testname2);

    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', testname2);
    cy.get('.settings-buttons .open-icon').trigger('click');
    cy.get('#settings-delete-user').trigger('click');
    cy.get('#confirm-accept').trigger('click');
    cy.wait(500);
    cy.location('pathname', { timeout: 5000 }).should('contain', '/login');
  });
});