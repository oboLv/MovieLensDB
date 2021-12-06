﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieLensDB.DataModels
{
    public class MovieGenre
    {
    public int Id {get;set;}
    public virtual Movie Movie { get; set; }
    public virtual Genre Genre { get; set; }
    }
}
