import { environment } from "src/environments/environment";

// ***********************************************
// This example namespace declaration will help
// with Intellisense and code completion in your
// IDE or Text Editor.
// ***********************************************
// declare namespace Cypress {
//   interface Chainable<Subject = any> {
//     customCommand(param: any): typeof customCommand;
//   }
// }
//
// function customCommand(param: any): void {
//   console.warn(param);
// }
//
// NOTE: You can use it like so:
// Cypress.Commands.add('customCommand', customCommand);
//
// ***********************************************
// This example commands.js shows you how to
// create various custom commands and overwrite
// existing commands.
//
// For more comprehensive examples of custom
// commands please read more here:
// https://on.cypress.io/custom-commands
// ***********************************************
//
//
// -- This is a parent command --
// Cypress.Commands.add("login", (email, password) => { ... })
//
//
// -- This is a child command --
// Cypress.Commands.add("drag", { prevSubject: 'element'}, (subject, options) => { ... })
//
//
// -- This is a dual command --
// Cypress.Commands.add("dismiss", { prevSubject: 'optional'}, (subject, options) => { ... })
//
//
// -- This will overwrite an existing command --
// Cypress.Commands.overwrite("visit", (originalFn, url, options) => { ... })

function generateFixtures() {
  const uuid = () => Cypress._.random(0, 1e6);
  const uuid2 = () => Cypress._.random(0, 1e6);
  
  cy.writeFile('cypress/fixtures/users.json', {
    username1: `Test_Name${uuid()}`,
    username2: `Test_Name${uuid2()}`,
    lobbyName: `Test_Lobby${uuid()}`,
  });
}

function login(username: string, password: string) {
  cy.session(username, () => {
    let body = new URLSearchParams();

    body.set('username', username);
    body.set('password', password);
    body.set('grant_type', environment.grant_type);
    body.set('client_id', environment.client_id);
    body.set('client_secret', environment.client_secret);

    cy.request({
      method: 'POST',
      url: `${environment.baseUrl}/user/login`,
      body: body.toString(),
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      }
    }).then(({ body }) => {
      cy.setCookie(environment.token_name, body.access_token);
      cy.setCookie(environment.refresh_token_name, body.refresh_token);
    });
  });
  cy.visit('/login');
};

function register(username: string, password: string) {
  const body = {
    userName: username,
    firstName: username,
    lastName: username,
    email: `${username}@${username}.hu`,
    password: password,
    confirmedPassword: password,
  };
  cy.request({
    method: 'POST',
    url: `${environment.baseUrl}/user/registration`,
    body,
  }).then(() => {
    login(username, password);
  });
};

Cypress.Commands.add('login', login);
Cypress.Commands.add('register', register);
Cypress.Commands.add('generateFixtures', generateFixtures);