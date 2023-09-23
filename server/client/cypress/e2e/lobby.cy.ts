describe('Lobby tests', () => {
  before(() => {
    cy.generateFixtures();
    cy.fixture('users').then((users) => {
      cy.register(users.username1, users.username1);
      cy.register(users.username2, users.username2);
    });
  });

  beforeEach(() => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);
    });
  });

  after(() => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);
      cy.get('.settings-buttons .open-icon').trigger('click');
      cy.get('#settings-delete-user').trigger('click');
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.login(users.username2, users.username2);
      cy.get('.settings-buttons .open-icon').trigger('click');
      cy.get('#settings-delete-user').trigger('click');
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
    });
  });

  it('create lobby', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');

      cy.visit('/lobby');
      cy.get('#create-lobby').click();
      cy.get('#create-lobby-form #name')
        .type(users.lobbyName)
        .should('have.value', users.lobbyName);
      cy.get('#create-lobby-form #password')
        .type(users.lobbyName)
        .should('have.value', users.lobbyName);
      cy.get('#lobby-create-button').click();
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');
    });
  });

  it('join lobby', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');

      cy.visit('/lobby');
      cy.get('.lobby-item .name')
        .filter(`:contains("${users.lobbyName}")`)
        .should('have.length', 1)
        .click();
      cy.get('#join-lobby-form #password')
        .type(users.lobbyName)
        .should('have.value', users.lobbyName);
      cy.get('#lobby-join-button').click();
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');
    });
  });

  it('redirect lobby', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');
      cy.get('#lobby-banner').should('have.text', users.lobbyName);

      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');
      cy.get('#lobby-banner').should('have.text', users.lobbyName);
    });
  });

  it('leave lobby', () => {
    cy.fixture('users').then((users) => {
      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');

      cy.get('#lobby-leave-button').click();
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should(
        'not.contain',
        '/lobby/'
      );

      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');

      cy.get('.player-item').should('have.length', 1);

      cy.get('#lobby-leave-button').click();
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should(
        'not.contain',
        '/lobby/'
      );

      cy.visit('/lobby');
      cy.get('.lobby-list-card').should('not.contain', users.lobbyName);
    });
  });

  it('delete lobby', () => {
    cy.fixture('users').then((users) => {
      // Create lobby
      cy.login(users.username1, users.username1);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');

      cy.visit('/lobby');
      cy.get('#create-lobby').click();
      cy.get('#create-lobby-form #name')
        .type(users.lobbyName)
        .should('have.value', users.lobbyName);
      cy.get('#create-lobby-form #password')
        .type(users.lobbyName)
        .should('have.value', users.lobbyName);
      cy.get('#lobby-create-button').click();
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');

      // Join lobby
      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');

      cy.visit('/lobby');
      cy.get('.lobby-item .name')
        .filter(`:contains("${users.lobbyName}")`)
        .should('have.length', 1)
        .click();
      cy.get('#join-lobby-form #password')
        .type(users.lobbyName)
        .should('have.value', users.lobbyName);
      cy.get('#lobby-join-button').click();
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');

      // Delete lobby
      cy.login(users.username1, users.username1);
      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/lobby/');

      cy.get('#lobby-leave-button').click();
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should(
        'not.contain',
        '/lobby/'
      );

      cy.login(users.username2, users.username2);

      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');

      cy.visit('/lobby');
      cy.get('.lobby-list-card').should('not.contain', users.lobbyName);
    });
  });
});
