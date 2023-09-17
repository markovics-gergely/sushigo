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
  cy.visit('/');
};

Cypress.Commands.add('login', login);