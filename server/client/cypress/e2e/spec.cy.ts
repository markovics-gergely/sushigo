describe('My First Test', () => {
  it('Visits the initial project page', () => {
    cy.login('Test_Name381488', 'Test_Name381488')
    cy.visit('/login');
    cy.location('pathname', { timeout: 5000 }).should('contain', '/home');
    cy.get('#home-username').should('have.text', 'Test_Name381488');
    cy.get('.settings-buttons .open-icon').trigger('click');
    cy.get('#settings-delete-user').trigger('click');
    cy.get('#confirm-accept').trigger('click');
    cy.wait(500);
    cy.location('pathname', { timeout: 5000 }).should('contain', '/login');
  });
})
