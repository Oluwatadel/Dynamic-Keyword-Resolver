In one of my projects, I needed to process files hosted on an FTP server. The file names on the server include dates, and my task required downloading files corresponding to the previous day.

I already have a generic FTP downloader in place, which can download any specified file and upload it to an S3 bucket for further processing. The challenge was specifying "yesterday" in the file name dynamically, as the downloader requires an exact file name to locate and download the file.

To solve this, I envisioned a lightweight NuGet package that could parse file names containing date-related keywords and expand them into actual file names. The package would allow me to specify keywords like YESTERDAY or NOW-1d in the file name and resolve them to their correct values.

Requirements for the Package

1. Keyword Parsing: The package should interpret and expand a variety of date-related keywords into actual date and time values.


2. Localization Support: The package must support date and time formatting based on different cultures or locales.


3. Flexibility: It should handle a wide range of date-related keywords, such as:

NOW, YESTERDAY, TODAY

Relative offsets like NOW-1d, NOW+2h

Specific formats like Format(NOW, "yyyy-MM-dd")
4. . Lightweight: The package should remain minimal in size while being efficient and easy to integrate into existing projects.



This parser would simplify the process of dynamically resolving file names and make it reusable across similar projects.