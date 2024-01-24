// See https://aka.ms/new-console-template for more information
using AutoDialer;
using AutoDialer.Infrastructure;
using AutoDialer.Repositories;
using AutoDialer.services;

var orchestrator = new DialingOrchestratorService(new DialService(new FakePhoneCallingAgent()), new PhoneRepository());
await orchestrator.Start();