import prompts from "prompts";

const passwordQuestions: prompts.PromptObject[] = [
  {
    name: "password",
    type: "password",
    message: "Password:",
  },
  {
    name: "confirmPassword",
    type: "password",
    message: "Confirm password:",
  },
];

export const run = async () => {
  const { user } = await prompts({
    name: "user",
    type: "text",
    message: "User name:",
  });

  let passwordAnswers = await prompts(passwordQuestions);
  while (passwordAnswers.password !== passwordAnswers.confirmPassword) {
    console.error("Password does not match, fill it again");
    passwordAnswers = await prompts(passwordQuestions);
  }

  // TODO: Insert into DB and disconnect it
  console.log(`User ${user} created!`);
};
