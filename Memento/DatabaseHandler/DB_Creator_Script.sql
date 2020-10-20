CREATE TABLE "Card_Table" (
	"card_id"	INTEGER NOT NULL,
	"image_path"	TEXT,
	"word"	TEXT NOT NULL,
	"transcription"	TEXT,
	"description"	TEXT NOT NULL,
	"difficulty_level"	TEXT NOT NULL,
	PRIMARY KEY("card_id" AUTOINCREMENT)
);

CREATE TABLE "Deck_Table" (
	"deck_id"	INTEGER NOT NULL,
	"tag_name"	TEXT,
	"deck_name"	TEXT NOT NULL,
	"number_of_cards"	INT NOT NULL,
	PRIMARY KEY("deck_id" AUTOINCREMENT),
	FOREIGN KEY("tag_name") REFERENCES "Tag_Table"("tag_name")
)

CREATE TABLE "Deck_To_Card_Table"(
	"card_id"	INTEGER NOT NULL,
	"deck_id"	INTEGER NOT NULL,
	PRIMARY KEY("card_id","deck_id")
	FOREIGN KEY("card_id") REFERENCES "Card_Table"("card_id")
	FOREIGN KEY("deck_id") REFERENCES "Deck_Table"("deck_id")
)

CREATE TABLE "Tag_To_Card_Table" (
	"tag_name"	TEXT NOT NULL,
	"card_id"	INT NOT NULL,
	PRIMARY KEY("tag_name","card_id"),
	FOREIGN KEY("card_id") REFERENCES "Card_Table"("card_id")
)