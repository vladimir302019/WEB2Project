import React, { useEffect, useState } from 'react'
import Button from '@mui/material/Button';
import Card from '@mui/material/Card';
import CardMedia from '@mui/material/CardMedia';
import Grid from '@mui/material/Grid';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import { TextField } from '@mui/material';
import { getAllArticlesAction } from '../../Store/articleSlice';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { addToCart } from '../../Store/cartSlice';

export default function Products() {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const articles = useSelector((state) => state.article.allArticles.map(article => ({ ...article, quantity: 0 })));
    const [isInitial, setIsInitial] = useState(true);
    const [quantity, setQuantity] = useState({});


    const user = useSelector((state) => state.user.user);
    let isAdmin = false;
    if(user.type === 2){
         isAdmin = true;
    }

    const img = "data:image/*;base64,";
    const imgUrl = process.env.PUBLIC_URL +'/no-image.jpg';

    const handleAddArticle = (article) => {
        const updatedQuantity = { ...quantity };
        updatedQuantity[article.id] = quantity[article.id] ? quantity[article.id]  : 0;
        setQuantity(updatedQuantity);
      
        dispatch(addToCart({ ...article, quantity: updatedQuantity[article.id] }));

        setQuantity({ ...quantity, [article.id]: 0 });
      };
      
    useEffect(()=> {
        if(!isInitial) {
          return;
        }

        const execute = () => {
          dispatch(getAllArticlesAction());
        };
        
        execute();
        setIsInitial(false);

      }, [isInitial, articles]);

      if(isInitial){
        return <div>Loading ...</div>
      }else
  return (
    <Container sx={{ py: 3 }}>
    {articles.map((article) => (
      <Grid item key={article.id} xs={12} sm={12} md={12} mt={2}>
        <Card
          sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'stretch',
            padding: '10px',
          }}
        >
          <div style={{ display: 'flex', alignItems: 'flex-start', justifyContent: 'space-between' }}>
            <div style={{ width: '20%' }}>
              <CardMedia
                component="img"
                src={article.photoUrl !== "" ? img+article.photoUrl : imgUrl}
                alt="Product Image"
                sx={{ height: '100%' }}
              />
            </div>
            <div style={{ flex: '1', display: 'flex', flexDirection: 'column', justifyContent: 'space-between', marginLeft: '5%' }}>
              <div style={{ alignSelf: 'flex-start' }}>
                <Typography gutterBottom variant="h4" component="h2" style={{ textAlign: 'center'}}>
                  {article.name}
                </Typography>
              </div>
              <div>
                <Typography variant="h6" style={{ textAlign: 'left', alignSelf: 'center' }}>
                  {article.description}
                </Typography>
              </div>
            </div>
            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end', marginLeft: 3 }}>
            <Typography gutterBottom variant="h6" style={{ textAlign: 'right' }}>
                Price: {article.price}.00 din
              </Typography>

              <TextField
                type="number"
                variant="outlined"
                size="small"
                sx={{ marginBottom: 2 }}
                inputProps={{ max: article.maxQuantity, min: 0 }}
                onChange={(e) => setQuantity({ ...quantity, [article.id]: parseInt(e.target.value) })}
                value={quantity[article.id] || 0}
              />
              <Button variant="contained" size="medium" color="primary" onClick={() => handleAddArticle(article)} disabled={isAdmin}>
                Add
              </Button>
            </div>
          </div>
        </Card>
      </Grid>
    ))}
  </Container>
  

  
  
  )
}