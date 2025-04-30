Instructions to run my project:

Once you run, you will be prompted to create an account. Once you create this account, a current bug has it to where you will see "Access Denied" on anything you try to create. To fix this, you must first Log Out of the account, then hit "Resend Email Confirmation".
Type in the email that you just created, and hit Resend. Once it says "Verification email sent. Please check your email", you are good to go. Log back into the account, and everything should function as normal.

One other error that has happened when cloning the project to another machine, is an error that says "invalid column name: UserId". I tried implementing a fix to this, which means you hopefully should not have to worry about this. If you do run into this, however, once you Add a new Migration, replace the "Up" method with the following:

protected override void Up(MigrationBuilder migrationBuilder)
{
  migrationBuilder.AddColumn<string>(
    name: "UserId",
    table: "Games",
    nullable: false);
}

Then Update the database as normal, and everything should work as intended.


AI Disclosure:
I created about 85% of my project during Spring Break, and doing so meant I had no access to lecture notes that were available later on in the course. Because of this, I relied on AI tools and general Google searches to help me understand and implement features such as AJAX, and utilizing a many to many relationship to create a complex function that used multiple entities. There were multiple times I asked an AI tool to explain something like AJAX to me, give me an example on how it could be implemented in an isolated example (not related to my project) and explain certain errors that I would receive (not correct them), which sped up my overall productivity on the project. One other instance I can think of is having AI explain the WCAG 2.1 AA standards, and using this I tried to comply with these standards in my project. Do note, I did **not** use AI to generate direct solutions for my assignment. The main tool I used was ChatGPT.

Accessibility Principles
My application follows WCAG 2.1 AA accessibility principles. Some of my key implementations include:
-Proper color contrast between text and background
-Many Inputs contain Drop Down boxes
-Links are visually distinguishable from surrounding text, utilizing underlining.
-Text has proper contrast ratios

Final Notes:
As stated above, I did a lot of the work in my term project before some lectures were available to us, so the AJAX functions are implemented, but not exactly in the way you showed us. I didn't create a seperate JS file in wwwroot, I implemented the scripts for it inside of the "Create" views in both RankEntry and Game. I completed this using jQuery for AJAX. Hopefully this isn't an issue, but I decided to note this in my ReadMe anyways.
