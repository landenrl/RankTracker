Instructions to run my project:

Once you run, you will be prompted to create an account. Once you create this account, a current bug has it to where you will see "Access Denied" on anything you try to create. To fix this, you must first Log Out of the account, then hit "Resend Email Confirmation".
Type in the email that you just created, and hit Resend. Once it says "Verification email sent. Please check your email", you are good to go. Log back into the account, and everything should function as normal.

One other error that has happened when cloning the project to another machine, is an error that says "invalid column name: UserId". I tried implementing a fix to this, which means you hopefully should not have to worry about this. If you do run into this, however, once
you Add a new Migration, replace the "Up" method with the following:

protected override void Up(MigrationBuilder migrationBuilder)
{
  migrationBuilder.AddColumn<string>(
    name: "UserId",
    table: "Games",
    nullable: false);
}

Then Update the database as normal, and everything should work as intended.

