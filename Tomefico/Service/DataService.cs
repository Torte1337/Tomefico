using System.Collections.Specialized;
using Microsoft.EntityFrameworkCore;
using Tomefico.Data;
using Tomefico.Enums;
using Tomefico.Models;

namespace Tomefico.Service;

public class DataService
{
    private readonly TomeContext context;
    private readonly LogService logService;
    public DataService(TomeContext context, LogService logService)
    {
        this.context = context;
        this.logService = logService;

        _ = OnCheckAndCreateDatabase();
    }
    private async Task OnCheckAndCreateDatabase()
    {
        try
        {
            var created = await context.Database.EnsureCreatedAsync();
            if (created)
                await logService.OnLog("Datenbank erstellt", "SQLite-Datenbank wurde neu erstellt", DateTime.Now, LogStatus.Info);
            else
                Console.WriteLine("[DEBUG INFO] Datenbank vorhaden");
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim erstellen / laden der Datenbank", ex.Message, DateTime.Now, LogStatus.Error);
        }
    }
    /// <summary>
    /// Methode speichert ein neues Buch in der Datenbank ab
    /// </summary>
    /// <param name="newBook"></param>
    /// <returns></returns>
    public async Task<bool> OnSaveNewBook(BookModel newBook)
    {
        try
        {
            if (newBook == null)
                return false;

            context.Books.Add(newBook);
            await context.SaveChangesAsync();
            await logService.OnLog("Neues Buch erfolgreich abgespeichert", $"Das Buch mit dem Titel {newBook.Title} wurde erfolgreich in der Datenbank gespeichert", DateTime.Now, LogStatus.Info);
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Speicher des neuen Buches in die Datenbank", ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }
    /// <summary>
    /// Methode aktualisiert ein bestehendes Buch in DB mit dem was übergeben und verglichen wurde
    /// </summary>
    /// <param name="updatedBook"></param>
    /// <returns></returns>
    public async Task<bool> OnUpdateBook(BookUpdateModel updatedBook)
    {
        try
        {
            if (updatedBook == null)
                return false;

            if (updatedBook.Id == Guid.Empty)
                return false;

            var existingBook = await context.Books.FindAsync(updatedBook.Id);
            if (existingBook == null)
                return false;

            bool hasChanges = false;

            if (!string.IsNullOrWhiteSpace(updatedBook.Title) && updatedBook.Title != existingBook.Title)
            {
                existingBook.Title = updatedBook.Title;
                hasChanges = true;
            }
            if (!string.IsNullOrWhiteSpace(updatedBook.Description) && updatedBook.Description != existingBook.Description)
            {
                existingBook.Description = updatedBook.Description;
                hasChanges = true;
            }
            if (!string.IsNullOrWhiteSpace(updatedBook.Notes) && updatedBook.Notes != existingBook.Notes)
            {
                existingBook.Notes = updatedBook.Notes;
                hasChanges = true;
            }
            if (updatedBook.PersonalRating.HasValue && updatedBook.PersonalRating.Value != existingBook.PersonalRating)
            {
                existingBook.PersonalRating = updatedBook.PersonalRating.Value;
                hasChanges = true;
            }
            if (updatedBook.CoverImage != null && !updatedBook.CoverImage.SequenceEqual(existingBook.CoverImage ?? Array.Empty<byte>()))
            {
                existingBook.CoverImage = updatedBook.CoverImage;
                hasChanges = true;
            }
            if (updatedBook.Status.HasValue && updatedBook.Status.Value != existingBook.Status)
            {
                existingBook.Status = updatedBook.Status.Value;
                hasChanges = true;
            }
            if (updatedBook.StartedReadingAt.HasValue && updatedBook.StartedReadingAt.Value != existingBook.StartedReadingAt)
            {
                existingBook.StartedReadingAt = updatedBook.StartedReadingAt.Value;
                hasChanges = true;
            }
            if (updatedBook.FinischedReadingAt.HasValue && updatedBook.FinischedReadingAt.Value != existingBook.FinischedReadingAt)
            {
                existingBook.FinischedReadingAt = updatedBook.FinischedReadingAt.Value;
                hasChanges = true;
            }

            if (!hasChanges)
            {
                await logService.OnLog("Keine Änderungen vorgenommen", "Es wurden keine Änderungen an dem bestehenden Eintrag gemacht", DateTime.Now, LogStatus.Info);
                return false;
            }

            context.Books.Update(existingBook);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Aktualisieren eines Buches", updatedBook.Title + " - " + ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }
    /// <summary>
    /// Methode wechselt den Status von einem ausgewählten Buch, wenn das Buch auf "fertig gelesen" gestellt wird, wird das FinishedReadingAt Datum auf jetzt gesetzt
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="newStatus"></param>
    /// <returns></returns>
    public async Task<bool> OnChangeStatus(Guid Id, BookStatus newStatus)
    {
        try
        {
            if (Id == Guid.Empty)
                return false;

            var existing = await context.Books.FindAsync(Id);
            if (existing == null)
                return false;

            if (newStatus != existing.Status)
            {
                if (newStatus == BookStatus.Finished)
                    existing.FinischedReadingAt = DateTime.Now;

                existing.Status = newStatus;
            }

            context.Books.Update(existing);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim wechsel des Status", ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }
    /// <summary>
    /// Methode löscht ein bestehendes Buch mittels der ausgewählten ID
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public async Task<bool> OnDeleteBook(Guid Id)
    {
        try
        {
            var existingBook = await context.Books.FindAsync(Id);
            if (existingBook == null)
                return false;

            context.Books.Remove(existingBook);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Löschen eines Buches", ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }
    /// <summary>
    /// Methode lädt alle Bücher, die noch zu lesen sind
    /// </summary>
    /// <returns></returns>
    public async Task<List<BookModel>> OnLoadToReadList()
    {
        try
        {
            var result = await context.Books.Where(x => x.Status == BookStatus.ToRead).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Laden der Bücherliste (noch zu lesen)", ex.Message, DateTime.Now, LogStatus.Error);
            return new List<BookModel>();
        }
    }
    /// <summary>
    /// Methode lädt alle Bücher, die gerade gelesen werden
    /// </summary>
    /// <returns></returns>
    public async Task<List<BookModel>> OnLoadReadingList()
    {
        try
        {
            var result = await context.Books.Where(x => x.Status == BookStatus.Reading).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Laden der Bücherliste (am lesen)", ex.Message, DateTime.Now, LogStatus.Error);
            return new List<BookModel>();
        }
    }
    /// <summary>
    /// Methode lädt alle Bücher, die fertig gelesen wurden
    /// </summary>
    /// <returns></returns>
    public async Task<List<BookModel>> OnLoadFinishedList()
    {
        try
        {
            var result = await context.Books.Where(x => x.Status == BookStatus.Finished).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Laden der Bücherliste (fertig gelesen)", ex.Message, DateTime.Now, LogStatus.Error);
            return new List<BookModel>();
        }
    }
    /// <summary>
    /// Methode lädt alle Bücher, die auf der Wunschliste sind
    /// </summary>
    /// <returns></returns>
    public async Task<List<BookModel>> OnLoadWishList()
    {
        try
        {
            var result = await context.Books.Where(x => x.Status == BookStatus.Wishlist).ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Laden der Bücherliste (wunschliste)", ex.Message, DateTime.Now, LogStatus.Error);
            return new List<BookModel>();
        }
    }
    /// <summary>
    /// Methode lädt alle Bücher aus der Datenbank
    /// </summary>
    /// <returns></returns>
    public async Task<List<BookModel>> OnLoadAll()
    {
        try
        {
            var result = await context.Books.ToListAsync();
            return result;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Laden der Bücherliste", ex.Message, DateTime.Now, LogStatus.Error);
            return new List<BookModel>();
        }
    }


    /// <summary>
    /// Methode erstellt einen neuen Author
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<bool> OnAddAuthor(AuthorModel author)
    {
        try
        {
            if (author == null)
                return false;

            var exist = await context.Authors.FirstOrDefaultAsync(x => x.Firstname == author.Firstname && x.Surname == author.Surname);
            if (exist != null)
                return false;

            context.Authors.Add(author);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim Speichern des Authors", ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }
    /// <summary>
    /// Methode aktualisiert einen Author
    /// </summary>
    /// <param name="updateModel"></param>
    /// <returns></returns>
    public async Task<bool> OnUpdateAuthor(AuthorUpdateModel updateModel)
    {
        try
        {
            if (updateModel == null)
                return false;

            var exist = await context.Authors.FindAsync(updateModel.Id);
            if (exist == null)
                return false;

            bool hasChanges = false;

            if (!string.IsNullOrWhiteSpace(updateModel.Firstname) && updateModel.Firstname != exist.Firstname)
            {
                exist.Firstname = updateModel.Firstname;
                hasChanges = true;
            }
            if (!string.IsNullOrWhiteSpace(updateModel.Surname) && updateModel.Surname != exist.Surname)
            {
                exist.Surname = updateModel.Surname;
                hasChanges = true;
            }

            if (!hasChanges)
                return false;

            context.Authors.Update(exist);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim aktualisieren des Authors", ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }
    /// <summary>
    /// Methode löscht einen Author
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> OnDeleteAuthor(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return false;

            var exist = await context.Authors.FindAsync(id);
            if (exist == null)
                return false;

            context.Authors.Remove(exist);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim aktualisieren des Authors", ex.Message, DateTime.Now, LogStatus.Error);
            return false;
        }
    }

    /// <summary>
    /// Methode lädt alle Bücher des Authors
    /// </summary>
    /// <param name="author"></param>
    /// <returns></returns>
    public async Task<List<BookModel>> GetBookListByAuthor(AuthorModel author)
    {
        try
        {
            var authorExist = await context.Authors.FindAsync(author.Id);
            if (authorExist == null)
                return new List<BookModel>();

            var bookList = await context.Books.Where(x => x.Author.Id == authorExist.Id).ToListAsync();
            return bookList;
        }
        catch (Exception ex)
        {
            await logService.OnLog("Fehler beim laden der Bücher des Authors", ex.Message, DateTime.Now, LogStatus.Error);
            return new List<BookModel>();
        }
    }




}
