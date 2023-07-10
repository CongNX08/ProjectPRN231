INSERT INTO Genres (Description)
VALUES
    ('Action'),
    ('Comedy'),
    ('Drama'),
    ('Horror'),
    ('Romance'),
    ('Thriller'),
    ('Sci-Fi'),
    ('Adventure'),
    ('Animation'),
    ('Fantasy');

	INSERT INTO Persons (Fullname, Gender, Email, Password, Type, IsActive)
VALUES
    ('John Doe', 'Male', 'johndoe@example.com', 'password', 1, 1),
    ('Jane Smith', 'Female', 'janesmith@example.com', 'password', 2, 1),
    ('Michael Johnson', 'Male', 'michaeljohnson@example.com', 'password', 1, 0),
    ('Emily Davis', 'Female', 'emilydavis@example.com', 'password', 2, 1),
    ('David Brown', 'Male', 'davidbrown@example.com', 'password', 1, 1),
    ('Sarah Wilson', 'Female', 'sarahwilson@example.com', 'password', 2, 0),
    ('Robert Taylor', 'Male', 'roberttaylor@example.com', 'password', 1, 1),
    ('Jessica Anderson', 'Female', 'jessicaanderson@example.com', 'password', 2, 1),
    ('Christopher Lee', 'Male', 'christopherlee@example.com', 'password', 1, 1),
    ('Amanda Martin', 'Female', 'amandamartin@example.com', 'password', 2, 0);

	INSERT INTO Movies (Title, Year, Image, Description, GenreId, RatingPoint)
VALUES
    ('The Matrix', 1999, 'https://m.media-amazon.com/images/I/51EG732BV3L._AC_UF894,1000_QL80_.jpg', 'A mind-bending movie about virtual reality', 1, 4.5),
    ('Inception', 2010, 'https://i1-vnexpress.vnecdn.net/2022/03/18/inceptionjpg-1647590997.jpg?w=330&h=495&q=100&dpr=1&fit=crop&s=izavUyS8PASL98snOpBQnQ', 'A heist thriller set within the architecture of the mind', 1, 4.8),
    ('Titanic', 1997, 'https://pisces.bbystatic.com/image2/BestBuy_US/images/products/5401/5401027_so.jpg', 'A tragic love story set on the ill-fated RMS Titanic', 5, 4.2),
    ('The Shawshank Redemption', 1994, 'https://i.ytimg.com/vi/19THOH_dvxg/movieposter_en.jpg', 'Two imprisoned men bond over several years, finding solace and eventual redemption through acts of common decency', 3, 4.9),
    ('The Godfather', 1972, 'https://ntvb.tmsimg.com/assets/p6326_v_h8_be.jpg?w=960&h=540', 'The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son', 3, 4.7),
    ('The Dark Knight', 2008, 'https://images.moviesanywhere.com/bd47f9b7d090170d79b3085804075d41/c6140695-a35f-46e2-adb7-45ed829fc0c0.jpg', 'When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice', 1, 4.9),
    ('Avatar', 2009, 'https://m.media-amazon.com/images/M/MV5BZDA0OGQxNTItMDZkMC00N2UyLTg3MzMtYTJmNjg3Nzk5MzRiXkEyXkFqcGdeQXVyMjUzOTY1NTc@._V1_.jpg', 'A paraplegic Marine dispatched to the moon Pandora on a unique mission becomes torn between following his orders and protecting the world he feels is his home', 7, 4.3),
    ('Pulp Fiction', 1994, 'https://m.media-amazon.com/images/I/81wrPiZFKIL._RI_.jpg', 'The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption', 1, 4.6),
    ('Forrest Gump', 1994, 'https://m.media-amazon.com/images/M/MV5BNWIwODRlZTUtY2U3ZS00Yzg1LWJhNzYtMmZiYmEyNmU1NjMzXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg', 'The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other historical events unfold through the perspective of an Alabama man with an IQ of 75', 3, 4.8),
    ('The Avengers', 2012, 'https://m.media-amazon.com/images/M/MV5BMTc5MDE2ODcwNV5BMl5BanBnXkFtZTgwMzI2NzQ2NzM@._V1_.jpg', 'A team of superhumans, including Iron Man, Captain America, Thor, Hulk, Black Widow, and Hawkeye, must stop Loki and his army from enslaving humanity', 7, 4.4);

	INSERT INTO Rates (MovieId, PersonId, Comment, NumericRating, Time)
VALUES
    (1, 1, 'Great movie!', 4.8, GETDATE()),
    (2, 2, 'Mind-blowing!', 4.9, GETDATE()),
    (3, 3, 'A timeless classic', 4.5, GETDATE()),
    (4, 4, 'One of the best movies ever made', 4.9, GETDATE()),
    (5, 5, 'Powerful and gripping', 4.7, GETDATE()),
    (6, 6, 'The epitome of superhero movies', 4.8, GETDATE()),
    (7, 7, 'Visually stunning', 4.3, GETDATE()),
    (8, 8, 'Quentin Tarantino at his best', 4.6, GETDATE()),
    (9, 9, 'Heartwarming and inspiring', 4.8, GETDATE()),
    (10, 10, 'Epic and action-packed', 4.5, GETDATE());

