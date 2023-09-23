describe('My First Test', () => {
  before(() => {
    cy.generateFixtures();
  });

  it('Visits the initial project page', () => {
    cy.fixture('users').then((users) => {
      cy.register(users.username1, users.username1)
      cy.visit('/login');
      cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
      cy.get('#home-username').should('have.text', users.username1);
      cy.get('.settings-buttons .open-icon').trigger('click');
      cy.get('#settings-delete-user').trigger('click');
      cy.get('#confirm-accept').trigger('click');
      cy.wait(500);
      cy.location('pathname', { timeout: 5000 }).should('contain', '/login');
    });
  });
})
