# Rock-Paper-Scissors Arena

A console-based Rock-Paper-Scissors game implemented in C#. The application allows a user to play multiple battles against an AI opponent, tracks session statistics, and provides a colorful user interface with ASCII art.

---

## Features

* **Nickname and Age Verification**: User must enter a non-empty nickname and be at least 12 years old.
* **Session Statistics**: Tracks total battles played and total wins in the current session (reset on restart).
* **Interactive Menu**: Navigate options using the arrow keys and Enter.
* **Three-Round Battles**: Each battle consists of three rounds; best-of-three determines victory.
* **ASCII Art**: Visual representation of Rock, Paper, and Scissors.
* **Colored Output**: Consistent console colors for different sections and outcomes via a `WithColor` helper.
* **Randomized Messages**: Multiple encouragement or congratulation messages chosen at random.

## Usage

1. Launch the application.
2. Enter your **nickname** (cannot be empty).
3. Enter your **age** (must be 12 or older).
4. View session statistics and choose **Yes** or **No** to start a battle.
5. If **Yes**, select your weapon (Rock, Paper, or Scissors) using arrow keys and **Enter**.
6. Repeat for three rounds; view battle results and messages.
7. Statistics update, and you can choose to play again or exit.

---

## Game Rules

* **Rock** crushes **Scissors**.
* **Scissors** cut **Paper**.
* **Paper** covers **Rock**.
* Best of three rounds wins the battle.
