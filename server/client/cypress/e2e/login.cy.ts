const uuid = () => Cypress._.random(0, 1e6);
const testname = `Test_Name${uuid()}`;

describe('login', () => {
  beforeEach(() => {
    cy.visit('/');
    cy.get('.bx-palette').click();
    cy.get('.bx-sun', { timeout: 2000 }).click();
  });
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
  });
});