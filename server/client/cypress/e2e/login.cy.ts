describe('login', () => {
  before(() => {
    cy.generateFixtures();
  });

  it('register', () => {
    cy.fixture('users').then((users) => {
      cy.visit('/register');
      cy.wait(500);
      cy.get('#userName')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#firstNameField').click();
      cy.get('#firstName')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#lastNameField').click();
      cy.get('#lastName')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#emailField').click();
      cy.get('#email')
        .type(users.username1 + '@admin.hu')
        .should('have.value', users.username1 + '@admin.hu');
      cy.get('#passwordField').click();
      cy.get('#password')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#confirmedPasswordField').click();
      cy.get('#confirmedPassword')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#registerButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/login');

      cy.get('#username')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#password')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#loginButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
    });
  });

  it('register again', () => {
    cy.fixture('users').then((users) => {
      cy.visit('/register');
      cy.get('#userName')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#firstNameField').click();
      cy.get('#firstName')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#lastNameField').click();
      cy.get('#lastName')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#emailField').click();
      cy.get('#email')
        .type(users.username1 + '@admin.hu')
        .should('have.value', users.username1 + '@admin.hu');
      cy.get('#passwordField').click();
      cy.get('#password')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#confirmedPasswordField').click();
      cy.get('#confirmedPassword')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#registerButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/register');
      cy.get('.mat-mdc-snack-bar-label').should(
        'contain',
        'Username or e-mail is already taken (400)'
      );
    });
  });

  it('register another', () => {
    cy.fixture('users').then((users) => {
      cy.visit('/register');
      cy.get('#userName')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#firstNameField').click();
      cy.get('#firstName')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#lastNameField').click();
      cy.get('#lastName')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#emailField').click();
      cy.get('#email')
        .type(users.username2 + '@admin.hu')
        .should('have.value', users.username2 + '@admin.hu');
      cy.get('#passwordField').click();
      cy.get('#password')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#confirmedPasswordField').click();
      cy.get('#confirmedPassword')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#registerButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/login');

      cy.get('#username')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#password')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#loginButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username2);
    });
  });

  it('login', () => {
    cy.fixture('users').then((users) => {
      cy.visit('/login');
      cy.get('#username')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#password')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#loginButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
    });
  });

  it('login another', () => {
    cy.fixture('users').then((users) => {
      cy.visit('/login');
      cy.get('#username')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#password')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#loginButton').click();
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username2);
    });
  });

  it('logged in before', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
    });
  });

  it('send friend request', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('#friend-button').click();
      cy.get('#friend-add-button').click();
      cy.get('#add-friend-input')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#add-friend-input-button').click();
      cy.wait(500);
      cy.get(
        '#sent-list > mat-expansion-panel-header #mat-badge-content-1'
      ).should('have.text', '1');
      cy.get('#sent-list > mat-expansion-panel-header').click();
      cy.wait(500);
      cy.get(
        '#sent-list > mat-expansion-panel-header #mat-badge-content-1'
      ).should('have.text', '0');
      cy.get('.friend-name').should('contain', users.username2);

      cy.login(users.username2, users.username2);
      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username2);
      cy.get('#friend-button').click();
      cy.get(
        '#received-list > mat-expansion-panel-header #mat-badge-content-1'
      ).should('have.length', 0);
      cy.get('#received-list > mat-expansion-panel-header').click();
      cy.get('.friend-name').should('contain', users.username1);

      cy.get('.accept-button').trigger('click');
      cy.get(
        '#friend-list > mat-expansion-panel-header #mat-badge-content-0'
      ).should('have.text', '1');
      cy.get('#friend-list > mat-expansion-panel-header').click();
      cy.get('.friend-name').should('contain', users.username1);

      cy.login(users.username1, users.username1);
      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('#friend-button').click();
      cy.get(
        '#friend-list > mat-expansion-panel-header #mat-badge-content-0'
      ).should('have.text', '0');
      cy.get('#friend-list > mat-expansion-panel-header').click();
      cy.get('.friend-name').should('contain', users.username2);
    });
  });

  it('send friend request again', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('#friend-button').click();
      cy.get('#friend-add-button').click();
      cy.get('#add-friend-input')
        .type(users.username2)
        .should('have.value', users.username2);
      cy.get('#add-friend-input-button').click();
      cy.get('.mat-mdc-snack-bar-label').should(
        'contain',
        'Invalid friend request (400)'
      );
    });
  });

  it('send friend request to yourself', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('#friend-button').click();
      cy.get('#friend-add-button').click();
      cy.get('#add-friend-input')
        .type(users.username1)
        .should('have.value', users.username1);
      cy.get('#add-friend-input-button').click();
      cy.get('.mat-mdc-snack-bar-label').should(
        'contain',
        'Invalid friend request (400)'
      );
    });
  });

  it('send friend request to non-existent user', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('#friend-button').click();
      cy.get('#friend-add-button').click();
      cy.get('#add-friend-input')
        .type('non-existent')
        .should('have.value', 'non-existent');
      cy.get('#add-friend-input-button').click();
      cy.get('.mat-mdc-snack-bar-label').should(
        'contain',
        'Invalid friend request (400)'
      );
    });
  });

  it('remove friend', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('#friend-button').click();
      cy.get(
        '#friend-list > mat-expansion-panel-header #mat-badge-content-0'
      ).should('have.text', '0');
      cy.get('.friend-name').should('contain', users.username2);

      cy.get('.decline-button').trigger('click');
      cy.get('.friend-name').should('not.exist');

      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username2);
      cy.get('#friend-button').click();
      cy.get(
        '#friend-list > mat-expansion-panel-header #mat-badge-content-0'
      ).should('have.text', '0');
      cy.get('.friend-name').should('not.exist');
    });
  });

  after(() => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('.settings-buttons .open-icon').trigger('click');
      cy.get('#settings-delete-user').trigger('click');
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/login');

      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username2);
      cy.get('.settings-buttons .open-icon').trigger('click');
      cy.get('#settings-delete-user').trigger('click');
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/login');
    });
  });
});
